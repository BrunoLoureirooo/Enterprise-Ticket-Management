using backend.Application.Services.Contracts;
using backend.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers
{ 
    public class RoleController : BaseApiController
    {
        private readonly IServiceManager _service;

        public RoleController(IServiceManager service)
        {
            _service = service;                       
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetRoles()
        {
            var roles = await _service.RoleService.GetRolesAsync();
            return Ok(roles.Select(r => new { r.Id, r.Name }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IdentityRoleWithClaimsDto>> GetRole(string id)
        {
            try { return Ok(await _service.RoleService.GetRoleAsync(id)); }
            catch (KeyNotFoundException) { return NotFound(); }
        }

        [HttpPost]
        public async Task<ActionResult<object>> CreateRole([FromBody] CreateRoleDto dto)
        {
            var role = await _service.RoleService.CreateRoleAsync(dto.Name);
            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, new { role.Id, role.Name });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            try { await _service.RoleService.DeleteRoleAsync(id); return NoContent(); }
            catch (KeyNotFoundException) { return NotFound(); }
        }

        [HttpPut("{id}/permissions")]
        public async Task<IActionResult> UpdatePermissions(string id, [FromBody] UpdatePermissionsDto dto)
        {
            try
            {
                await _service.RoleService.UpdateRolePermissionsAsync(id, dto.Permissions);
                return NoContent();
            }
            catch (KeyNotFoundException) { return NotFound(); }
        }
    }

    public record CreateRoleDto(string Name);
    public record UpdatePermissionsDto(IEnumerable<string> Permissions);
}
