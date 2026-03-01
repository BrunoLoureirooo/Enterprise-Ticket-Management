using backend.Application.Services.Contracts;
using backend.Entities;
using backend.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace backend.Application.Services
{
    internal class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenRevocationService _revocation;
        private readonly IOptions<JwtConfiguration> _jwtConfig;

        public RoleService(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ITokenRevocationService revocation,
            IOptions<JwtConfiguration> jwtConfig)
        {
            _roleManager  = roleManager;
            _userManager  = userManager;
            _revocation   = revocation;
            _jwtConfig    = jwtConfig;
        }

        public Task<IEnumerable<ApplicationRole>> GetRolesAsync()
            => Task.FromResult(_roleManager.Roles.AsEnumerable());

        public async Task<IdentityRoleWithClaimsDto> GetRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId)
                ?? throw new KeyNotFoundException($"Role '{roleId}' not found.");

            var claims = await _roleManager.GetClaimsAsync(role);
            return new IdentityRoleWithClaimsDto(
                role.Id.ToString(),
                role.Name ?? string.Empty,
                claims.Select(c => c.Value));
        }

        public async Task<ApplicationRole> CreateRoleAsync(string roleName)
        {
            var role = new ApplicationRole { Name = roleName };
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
            return role;
        }

        public async Task DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId)
                ?? throw new KeyNotFoundException($"Role '{roleId}' not found.");

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
            if(usersInRole.Any())
                throw new InvalidOperationException($"Role '{roleId}' is in use.");

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        public async Task UpdateRolePermissionsAsync(string roleId, IEnumerable<string> permissions)
        {
            var role = await _roleManager.FindByIdAsync(roleId)
                ?? throw new KeyNotFoundException($"Role '{roleId}' not found.");

            var existing = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in existing)
                await _roleManager.RemoveClaimAsync(role, claim);

            var permList = permissions.Distinct().OrderBy(p => p).ToList();
            foreach (var perm in permList)
                await _roleManager.AddClaimAsync(role, new Claim("permissions", perm));

            var permHash = ComputePermHash(permList);
            var tokenLifetime = TimeSpan.FromMinutes(
                Convert.ToDouble(_jwtConfig.Value.Expires));

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
            foreach (var user in usersInRole)
            {
                await _revocation.RevokeAllUserTokensAsync(user.Id, tokenLifetime);
                await _revocation.SetPermissionsHashAsync(user.Id, permHash, tokenLifetime);
            }
        }

        private static string ComputePermHash(IEnumerable<string> sortedPermissions)
        {
            var input = string.Join(",", sortedPermissions);
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes).ToLower();
        }
    }
}
