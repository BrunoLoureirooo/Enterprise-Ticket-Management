using ticket.Entities.DataTransferObjects.Tickets;

namespace ticket.Application.Services.Contracts
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDto>> GetAllAsync();
        Task<IEnumerable<TicketDto>> GetScopedAsync(Guid userId);
        Task<IEnumerable<TicketDto>> GetByUserAsync(Guid userId);
        Task<TicketDto?> GetByIdAsync(Guid id);
        Task<TicketDto> CreateAsync(Guid userId, CreateTicketDto dto);
        Task<TicketDto?> UpdateAsync(Guid id, UpdateTicketDto dto);
        Task<TicketDto?> AssignAsync(Guid id, Guid assignedToId);
        Task<TicketDto?> CloseAsync(Guid id);
        Task<bool> DeleteAsync(Guid id);
    }
}
