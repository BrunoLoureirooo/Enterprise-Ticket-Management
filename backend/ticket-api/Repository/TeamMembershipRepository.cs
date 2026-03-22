using ticket.Entities.Models;
using ticket.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ticket.Repository
{
    public class TeamMembershipRepository(RepositoryContext context) : ITeamMembershipRepository
    {
        public async Task<IEnumerable<TeamMembership>> GetByUserAsync(Guid userId) =>
            await context.TeamMemberships.Where(m => m.UserId == userId).ToListAsync();

        public async Task<TeamMembership?> GetAsync(Guid userId, Guid teamId) =>
            await context.TeamMemberships.FirstOrDefaultAsync(m => m.UserId == userId && m.TeamId == teamId);

        public void Add(TeamMembership membership) => context.TeamMemberships.Add(membership);
        public void Remove(TeamMembership membership) => context.TeamMemberships.Remove(membership);
        public void Update(TeamMembership membership) => context.TeamMemberships.Update(membership);
    }
}
