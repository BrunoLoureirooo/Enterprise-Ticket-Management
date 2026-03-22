using Microsoft.AspNetCore.Mvc;
using teams.Application.Services.Contracts;
using teams.Entities.DataTransferObjects.Members;
using teams.Entities.DataTransferObjects.Teams;

namespace teams.API.Controllers
{
    public class TeamsController(IServiceManager service) : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teams = await service.TeamService.GetAllAsync();
            return Ok(teams);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var team = await service.TeamService.GetByIdAsync(id);
            return team is null ? NotFound() : Ok(team);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTeamDto dto)
        {
            var created = await service.TeamService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTeamDto dto)
        {
            var updated = await service.TeamService.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await service.TeamService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        [HttpPost("{id:guid}/members")]
        public async Task<IActionResult> AddMember(Guid id, [FromBody] AddMemberDto dto)
        {
            try
            {
                var team = await service.TeamService.AddMemberAsync(id, dto);
                return team is null ? NotFound() : Ok(team);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:guid}/members/{userId:guid}")]
        public async Task<IActionResult> RemoveMember(Guid id, Guid userId)
        {
            var team = await service.TeamService.RemoveMemberAsync(id, userId);
            return team is null ? NotFound() : Ok(team);
        }

        [HttpPatch("{id:guid}/members/{userId:guid}/leader")]
        public async Task<IActionResult> SetLeader(Guid id, Guid userId, [FromBody] bool isLeader)
        {
            var team = await service.TeamService.SetLeaderAsync(id, userId, isLeader);
            return team is null ? NotFound() : Ok(team);
        }
    }
}
