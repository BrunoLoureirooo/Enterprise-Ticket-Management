using AutoMapper;
using backend.Entities;
using backend.Entities.Models;
using backend.Application.Services.Contracts;
using backend.Entities.DataTransferObjects;
using backend.Entities.DataTransferObjects.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using backend.Entities.Constants;

namespace backend.Application.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        //Properties
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IOptions<JwtConfiguration> _configuration;
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly IClaimEnricher[] _claimEnrichers;
        private readonly ITokenRevocationService _revocation;


        //Contructors
        public AuthenticationService(ILoggerManager logger, IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IOptions<JwtConfiguration> configuration, IEnumerable<IClaimEnricher> claimEnrichers, ITokenRevocationService revocation)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _jwtConfiguration = _configuration.Value;
            _claimEnrichers = claimEnrichers.ToArray();
            _revocation = revocation;
        }


        //Public Functions
        public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration)
        {
            if(string.IsNullOrEmpty(userForRegistration?.Email) || string.IsNullOrEmpty(userForRegistration?.Password))
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "InvalidUser",
                    Description = "Invalid user"
                });
            
            var userCheck = await _userManager.FindByEmailAsync(userForRegistration?.Email!);
            if (userCheck != null)
            {
                _logger.LogWarn($"{nameof(RegisterUser)}: User with email {userForRegistration?.Email} already exists.");
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserAlreadyExists",
                    Description = "User already exists"
                });
            }
        
            var user = _mapper.Map<ApplicationUser>(userForRegistration);

            var result = await _userManager.CreateAsync(user, userForRegistration?.Password ?? "NA");
            if (result.Succeeded)
                await _userManager.AddToRolesAsync(user, userForRegistration?.Roles ?? []);

            return result;
        }

        public async Task<ApplicationUser?> ValidateUser(UserForAuthenticationDto userForAuth)
        {
            var user = await _userManager.FindByEmailAsync(userForAuth?.Username ?? "NA");

            var result = user != null && await _userManager.CheckPasswordAsync(user, userForAuth?.Password ?? "NA");
            if (!result)
                _logger.LogWarn($"{nameof(ValidateUser)}: Authentication failed. Wrong username or password.");

            return result ? user : null;
        }

        public async Task<TokenDto> CreateToken(ApplicationUser user, bool populateExp)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            if (populateExp)
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userManager.UpdateAsync(user);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            var tokenLifetime = TimeSpan.FromMinutes(Convert.ToDouble(_jwtConfiguration.Expires));

            // Track JTI so it can be revoked later (explicit logout or permission change)
            var jti = claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            await _revocation.TrackJtiAsync(user.Id, jti, tokenLifetime);

            // Write permissions hash — gateway compares this on every request
            var permHash = claims.First(c => c.Type == "perm_hash").Value;
            await _revocation.SetPermissionsHashAsync(user.Id, permHash, tokenLifetime);

            return new TokenDto(accessToken, refreshToken);
        }
        
        public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            var username = principal?.Identity?.Name ?? "NA";

            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new Exception("Bad Token");

            return await CreateToken(user, populateExp: false);
        }


        public async Task<bool> Authorize(AuthorizationRequestDto request)
        {
            try
            {
                // Resolve the required permission first
                var requiredPermission = PermissionResolver.Resolve(request.Path, request.Method);
                if (string.IsNullOrEmpty(requiredPermission))
                {
                    _logger.LogWarn($"{nameof(Authorize)}: No permission mapped for {request.Method} {request.Path}");
                    return false; // Default deny if no mapping
                }

                // Public routes don't require a token
                if (requiredPermission == "anonymous")
                    return true;

                var principal = GetPrincipalFromExpiredToken(request.Token);
                if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
                    return false;

                var username = principal.Identity.Name;
                if (string.IsNullOrEmpty(username))
                    return false;
                
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                    return false;

                var roles = await _userManager.GetRolesAsync(user);
                bool hasPermission = false;

                foreach (var roleName in roles)
                {
                    var roleEntity = await _roleManager.FindByNameAsync(roleName);
                    if (roleEntity != null)
                    {
                        var roleClaims = await _roleManager.GetClaimsAsync(roleEntity);
                        if (roleClaims.Any(c => c.Type == "permission" && c.Value == requiredPermission))
                        {
                            hasPermission = true;
                            break;
                        }
                    }
                }

                if (!hasPermission)
                {
                    _logger.LogWarn($"{nameof(Authorize)}: User {username} does not have required permission {requiredPermission}");
                    return false;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Authorize)}: Token validation failed - {ex.Message}");
                return false;
            }
        }

        //Private Functions
        private SigningCredentials GetSigningCredentials()
        {
            var secretKey = Environment.GetEnvironmentVariable("SECRET")
                ?? throw new InvalidOperationException("JWT SECRET environment variable is not configured.");
            var key = Encoding.UTF8.GetBytes(secretKey);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        
        private async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,             user.Nome!),
                new Claim(ClaimTypes.Email,            user.Email!),
                new Claim(ClaimTypes.NameIdentifier,   user.Id.ToString()),
                new Claim(ClaimTypes.Uri,              user.ImageUrl ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var roles = await _userManager.GetRolesAsync(user);
            var allPermissions = new HashSet<string>();

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));

                var roleEntity = await _roleManager.FindByNameAsync(role);
                if (roleEntity == null) continue;

                var roleClaims = await _roleManager.GetClaimsAsync(roleEntity);
                foreach (var rc in roleClaims)
                {
                    claims.Add(new Claim("permissions", rc.Value));
                    allPermissions.Add(rc.Value);
                }
            }

            // Contextual attributes from external services (dept, org_id, etc.)
            // Each IClaimEnricher handles one domain service. Register new ones in Program.cs.
            foreach (var enricher in _claimEnrichers)
            {
                var enriched = await enricher.EnrichAsync(user.Id);
                claims.AddRange(enriched);
            }

            var permissionString = string.Join(",", allPermissions.OrderBy(p => p));
            var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(permissionString));
            claims.Add(new Claim("perm_hash", Convert.ToHexString(hashBytes).ToLower()));

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken
            (
                issuer: _jwtConfiguration.ValidIssuer,
                audience: _jwtConfiguration.ValidAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires)),
                signingCredentials: signingCredentials
            );
            return tokenOptions;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET") ?? throw new InvalidOperationException("JWT SECRET environment variable is not configured."))),
                ValidateLifetime = false, 
                ValidIssuer = _jwtConfiguration.ValidIssuer,
                ValidAudience = _jwtConfiguration.ValidAudience
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception("Invalid token");
            }
            return principal;
        }
    }
}