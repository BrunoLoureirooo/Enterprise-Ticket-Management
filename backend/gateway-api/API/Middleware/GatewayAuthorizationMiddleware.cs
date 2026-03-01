using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;

namespace gateway_api.API.Middleware
{
    public class GatewayAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;
        private readonly ILogger<GatewayAuthorizationMiddleware> _logger;
        private readonly string _identityApiUrl;

        public GatewayAuthorizationMiddleware(
            RequestDelegate next, 
            IHttpClientFactory httpClientFactory, 
            IMemoryCache cache, 
            ILogger<GatewayAuthorizationMiddleware> logger,
            IConfiguration configuration)
        {
            _next = next;
            _httpClientFactory = httpClientFactory;
            _cache = cache;
            _logger = logger;
            _identityApiUrl = configuration.GetValue<string>("IdentityApiUrl") ?? "https://localhost:5001";
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last() ?? string.Empty;
            var path = context.Request.Path.Value ?? string.Empty;
            var method = context.Request.Method;

            string userId = ExtractUserIdFromToken(token);

            // Generate Cache Key
            var cacheKey = GenerateCacheKey(token, path, method);

            if (_cache.TryGetValue(cacheKey, out bool isAuthorized))
            {
                if (!isAuthorized)
                {
                    context.Response.StatusCode = 403;
                    return;
                }

                ForwardRequest(context, userId);
                await _next(context);
                return;
            }

            // Call Identity API PDP
            var client = _httpClientFactory.CreateClient("pdp");
            
            var requestPayload = new
            {
                Token = token,
                Path = path,
                Method = method
            };

            var content = new StringContent(JsonSerializer.Serialize(requestPayload), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync($"{_identityApiUrl}/api/Auth/authorize", content);

                if (response.IsSuccessStatusCode)
                {
                    // The API just returns Ok(bool) implicitly empty or true
                    _cache.Set(cacheKey, true, TimeSpan.FromSeconds(30));
                    ForwardRequest(context, userId);
                    await _next(context);
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error communicating with PDP (Identity API)");
            }

            // Fallback deny
            _cache.Set(cacheKey, false, TimeSpan.FromSeconds(30));
            context.Response.StatusCode = 403;
        }

        private void ForwardRequest(HttpContext context, string userId)
        {
            // Strip the Authorization header so downstream doesn't see it/need to parse it
            context.Request.Headers.Remove("Authorization");
            
            // Inject X-User-Id for downstream
            context.Request.Headers["X-User-Id"] = userId;
        }

        private string GenerateCacheKey(string token, string path, string method)
        {
            var tokenHash = string.IsNullOrEmpty(token) ? "empty" : Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
            return $"{tokenHash}:{path}:{method}";
        }

        private string ExtractUserIdFromToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return "anonymous";

            try
            {
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                // "nameid" is the standard claim type for NameIdentifier
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid" || c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
                return userIdClaim?.Value ?? "anonymous";
            }
            catch
            {
                return "anonymous";
            }
        }
    }
}
