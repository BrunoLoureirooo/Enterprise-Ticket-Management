using ticket.Entities.Models;
using ticket.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ticket.Repository
{
    public class TicketRepository(RepositoryContext context) : ITicketRepository
    {
        public async Task<IEnumerable<Ticket>> GetAllAsync() =>
            await context.Tickets.OrderByDescending(t => t.CreatedAt).ToListAsync();

        public async Task<IEnumerable<Ticket>> GetByTeamsAsync(IEnumerable<Guid> teamIds) =>
            await context.Tickets
                .Where(t => t.TeamId.HasValue && teamIds.Contains(t.TeamId.Value))
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Ticket>> GetByUserAsync(Guid userId) =>
            await context.Tickets
                .Where(t => t.AssignedToId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

        public async Task<Ticket?> GetByIdAsync(Guid id) =>
            await context.Tickets.FirstOrDefaultAsync(t => t.Id == id);

        public void Create(Ticket ticket) => context.Tickets.Add(ticket);

        public void Update(Ticket ticket) => context.Tickets.Update(ticket);

        public void Delete(Ticket ticket) => context.Tickets.Remove(ticket);
    }
}
