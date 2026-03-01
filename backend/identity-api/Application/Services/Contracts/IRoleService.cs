using backend.Entities.Models;
using System.Security.Claims;

namespace backend.Application.Services.Contracts
{
    public interface IRoleService
    {
        Task<IEnumerable<ApplicationRole>> GetRolesAsync();
        Task<IdentityRoleWithClaimsDto> GetRoleAsync(string roleId);
        Task<ApplicationRole> CreateRoleAsync(string roleName);
        Task DeleteRoleAsync(string roleId);

        Task UpdateRolePermissionsAsync(string roleId, IEnumerable<string> permissions);
    }

    public record IdentityRoleWithClaimsDto(string Id, string Name, IEnumerable<string> Permissions);
}
