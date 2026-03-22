using teams.Entities.Models;
using teams.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace teams.Repository
{
    public class TeamRepository(RepositoryContext context) : ITeamRepository
    {
        public async Task<IEnumerable<Team>> GetAllAsync() =>
            await context.Teams.Include(t => t.Members).ThenInclude(m => m.User).OrderBy(t => t.Name).ToListAsync();

        public async Task<Team?> GetByIdAsync(Guid id) =>
            await context.Teams.Include(t => t.Members).ThenInclude(m => m.User).FirstOrDefaultAsync(t => t.Id == id);

        public async Task<TeamMember?> GetMemberAsync(Guid teamId, Guid userId) =>
            await context.TeamMembers.FirstOrDefaultAsync(m => m.TeamId == teamId && m.UserId == userId);

        public void Create(Team team) => context.Teams.Add(team);
        public void Update(Team team) => context.Teams.Update(team);
        public void Delete(Team team) => context.Teams.Remove(team);
        public void AddMember(TeamMember member) => context.TeamMembers.Add(member);
        public void RemoveMember(TeamMember member) => context.TeamMembers.Remove(member);
        public void UpdateMember(TeamMember member) => context.TeamMembers.Update(member);
    }
}
