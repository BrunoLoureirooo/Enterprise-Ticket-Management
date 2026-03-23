using Microsoft.AspNetCore.Mvc;
using ticket.Application.Services.Contracts;
using ticket.Entities.DataTransferObjects.Tickets;

namespace ticket.API.Controllers
{
    public class TicketController : BaseApiController
    {
        private readonly IServiceManager _service;

        public TicketController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var userId = GetUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var stats = await _service.TicketService.GetStatsAsync(userId, IsAdmin());
            return Ok(stats);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (IsAdmin())
            {
                var all = await _service.TicketService.GetAllAsync();
                return Ok(all);
            }

            var userId = GetUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var tickets = await _service.TicketService.GetScopedAsync(userId);
            return Ok(tickets);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMy()
        {
            var userId = GetUserId();
            if (userId == Guid.Empty) return Unauthorized();

            var tickets = await _service.TicketService.GetByUserAsync(userId);
            return Ok(tickets);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var ticket = await _service.TicketService.GetByIdAsync(id);
            return ticket is null ? NotFound() : Ok(ticket);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTicketDto dto)
        {
            var userId = GetUserId();
            if (userId == Guid.Empty) return Unauthorized();

            try
            {
                var created = await _service.TicketService.CreateAsync(userId, dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTicketDto dto)
        {
            try
            {
                var updated = await _service.TicketService.UpdateAsync(id, dto);
                return updated is null ? NotFound() : Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id:guid}/assign")]
        public async Task<IActionResult> Assign(Guid id, [FromBody] Guid assignedToId)
        {
            try
            {
                var updated = await _service.TicketService.AssignAsync(id, assignedToId);
                return updated is null ? NotFound() : Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id:guid}/close")]
        public async Task<IActionResult> Close(Guid id)
        {
            var updated = await _service.TicketService.CloseAsync(id);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.TicketService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
