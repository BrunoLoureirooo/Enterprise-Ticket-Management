using ticket.Entities.Models;

namespace ticket.Repository.Contracts
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetAllAsync();
        Task<IEnumerable<Ticket>> GetByTeamsAsync(IEnumerable<Guid> teamIds);
        Task<IEnumerable<Ticket>> GetByUserAsync(Guid userId);
        Task<Ticket?> GetByIdAsync(Guid id);
        void Create(Ticket ticket);
        void Update(Ticket ticket);
        void Delete(Ticket ticket);
    }
}
