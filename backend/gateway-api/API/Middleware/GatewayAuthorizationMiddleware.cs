using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace gateway_api.API.Middleware
{
    public class GatewayAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDatabase _redis;
        private readonly ILogger<GatewayAuthorizationMiddleware> _logger;
        private readonly TokenValidationParameters _tokenValidationParams;

        // Paths treated as public — no token required
        private static readonly HashSet<string> PublicPrefixes =
            new(StringComparer.OrdinalIgnoreCase) { "swagger", "health" };

        // Auth controller actions that don't require a token
        private static readonly HashSet<string> PublicAuthActions =
            new(StringComparer.OrdinalIgnoreCase) { "login", "register", "refresh" };

        public GatewayAuthorizationMiddleware(
            RequestDelegate next,
            IConnectionMultiplexer redis,
            ILogger<GatewayAuthorizationMiddleware> logger,
            IConfiguration configuration)
        {
            _next = next;
            _redis = redis.GetDatabase();
            _logger = logger;

            var secret = Environment.GetEnvironmentVariable("SECRET")
                ?? throw new InvalidOperationException("JWT SECRET env var is not set.");

            _tokenValidationParams = new TokenValidationParameters
            {
                ValidateIssuer           = true,
                ValidateAudience         = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime         = true,
                ValidIssuer    = configuration["JwtSettings:Issuer"]   ?? "TicketManagementAPI",
                ValidAudience  = configuration["JwtSettings:Audience"] ?? "TicketManagementClient",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ClockSkew = TimeSpan.FromSeconds(30),
                RoleClaimType = ClaimTypes.Role,
                NameClaimType = ClaimTypes.Name,
            };
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path   = context.Request.Path.Value ?? string.Empty;
            var method = context.Request.Method;
            var parts  = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

            // ── Public routes (no token needed) ────────────────────────────────
            if (IsPublicRoute(parts))
            {
                await _next(context);
                return;
            }

            // ── Extract + validate token ────────────────────────────────────────
            var token = context.Request.Headers["Authorization"]
                             .FirstOrDefault()?.Split(" ").Last() ?? string.Empty;

            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = 401;
                return;
            }

            ClaimsPrincipal principal;
            try
            {
                var handler = new JwtSecurityTokenHandler();
                principal = handler.ValidateToken(token, _tokenValidationParams, out _);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("JWT validation failed: {Msg}", ex.Message);
                context.Response.StatusCode = 401;
                return;
            }

            var userId   = principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var jti      = principal.FindFirstValue(JwtRegisteredClaimNames.Jti) ?? string.Empty;
            var permHash = principal.FindFirstValue("perm_hash") ?? string.Empty;
            var dept     = principal.FindFirstValue("dept") ?? string.Empty;

            var isAdmin = principal.Claims.Any(c =>
                (c.Type == ClaimTypes.Role || c.Type == "role") &&
                c.Value.Equals("Admin", StringComparison.OrdinalIgnoreCase));

            if (isAdmin)
            {
                ForwardRequest(context, userId, dept, principal);
                await _next(context);
                return;
            }

            if (!string.IsNullOrEmpty(jti))
            {
                var isBlocklisted = await _redis.KeyExistsAsync($"blocklist:{jti}");
                if (isBlocklisted)
                {
                    _logger.LogWarning("Revoked token presented (jti={Jti})", jti);
                    context.Response.StatusCode = 401;
                    return;
                }
            }

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(permHash))
            {
                var storedHash = await _redis.StringGetAsync($"userhash:{userId}");
                if (storedHash.HasValue && storedHash != permHash)
                {
                    _logger.LogWarning("Stale permissions token for user {UserId}", userId);
                    context.Response.StatusCode = 401;
                    return;
                }
            }

            var required = ResolveRequiredPermission(parts, method);
            if (required is not null)
            {
                var hasPermission = principal.Claims
                    .Any(c => c.Type == "permissions" && c.Value == required);

                if (!hasPermission)
                {
                    _logger.LogWarning(
                        "User {UserId} lacks permission '{Perm}' for {Method} {Path}",
                        userId, required, method, path);
                    context.Response.StatusCode = 403;
                    return;
                }
            }

            ForwardRequest(context, userId, dept, principal);
            await _next(context);
        }

        // ── Helpers ─────────────────────────────────────────────────────────────

        private static bool IsPublicRoute(string[] parts)
        {
            if (parts.Length == 0) return false;

            if (PublicPrefixes.Contains(parts[0])) return true;

            if (parts.Length >= 3
                && parts[0].Equals("api", StringComparison.OrdinalIgnoreCase)
                && parts[1].Equals("auth", StringComparison.OrdinalIgnoreCase)
                && PublicAuthActions.Contains(parts[2]))
                return true;

            return false;
        }

        private static string? ResolveRequiredPermission(string[] parts, string method)
        {
            if (parts.Length < 2) return null;
            if (!parts[0].Equals("api", StringComparison.OrdinalIgnoreCase)) return null;

            var resource = parts[1].ToLower();

            return method.ToUpper() switch
            {
                "GET"            => $"{resource}:read",
                "POST"           => $"{resource}:create",
                "PUT" or "PATCH" => $"{resource}:update",
                "DELETE"         => $"{resource}:delete",
                _                => null
            };
        }

        private static void ForwardRequest(HttpContext context, string userId, string dept, ClaimsPrincipal principal)
        {
            context.Request.Headers.Remove("Authorization");
            context.Request.Headers["X-User-Id"] = userId;

            if (!string.IsNullOrEmpty(dept))
                context.Request.Headers["X-User-Dept"] = dept;

            var permissions = principal.Claims
                .Where(c => c.Type == "permissions")
                .Select(c => c.Value);
            var permString = string.Join(",", permissions);
            if (!string.IsNullOrEmpty(permString))
                context.Request.Headers["X-User-Permissions"] = permString;
        }
    }
}
