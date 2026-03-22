using Microsoft.AspNetCore.Mvc;
using teams.Application.Services.Contracts;
using teams.Entities.DataTransferObjects.Projects;

namespace teams.API.Controllers
{
    public class ProjectsController(IServiceManager service) : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Guid? teamId)
        {
            var projects = teamId.HasValue
                ? await service.ProjectService.GetByTeamAsync(teamId.Value)
                : await service.ProjectService.GetAllAsync();
            return Ok(projects);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var project = await service.ProjectService.GetByIdAsync(id);
            return project is null ? NotFound() : Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto dto)
        {
            try
            {
                var created = await service.ProjectService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectDto dto)
        {
            var updated = await service.ProjectService.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await service.ProjectService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        [HttpPost("{id:guid}/teams/{teamId:guid}")]
        public async Task<IActionResult> LinkTeam(Guid id, Guid teamId)
        {
            try
            {
                var project = await service.ProjectService.LinkTeamAsync(id, teamId);
                return project is null ? NotFound() : Ok(project);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:guid}/teams/{teamId:guid}")]
        public async Task<IActionResult> UnlinkTeam(Guid id, Guid teamId)
        {
            var project = await service.ProjectService.UnlinkTeamAsync(id, teamId);
            return project is null ? NotFound() : Ok(project);
        }
    }
}
