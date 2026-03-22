using ticket.Entities.Models;
using ticket.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ticket.Repository
{
    public class SyncedUserRepository(RepositoryContext context) : ISyncedUserRepository
    {
        public async Task<SyncedUser?> GetAsync(Guid userId) =>
            await context.SyncedUsers.FirstOrDefaultAsync(u => u.UserId == userId);
        public async Task<bool> ExistsAsync(Guid userId) =>
            await context.SyncedUsers.AnyAsync(u => u.UserId == userId);
        public void Add(SyncedUser u)    => context.SyncedUsers.Add(u);
        public void Update(SyncedUser u) => context.SyncedUsers.Update(u);
        public void Remove(SyncedUser u) => context.SyncedUsers.Remove(u);
    }

    public class SyncedTeamRepository(RepositoryContext context) : ISyncedTeamRepository
    {
        public async Task<SyncedTeam?> GetAsync(Guid teamId) =>
            await context.SyncedTeams.FirstOrDefaultAsync(t => t.TeamId == teamId);
        public async Task<bool> ExistsAsync(Guid teamId) =>
            await context.SyncedTeams.AnyAsync(t => t.TeamId == teamId);
        public void Add(SyncedTeam t)    => context.SyncedTeams.Add(t);
        public void Update(SyncedTeam t) => context.SyncedTeams.Update(t);
        public void Remove(SyncedTeam t) => context.SyncedTeams.Remove(t);
    }

    public class SyncedProjectRepository(RepositoryContext context) : ISyncedProjectRepository
    {
        public async Task<SyncedProject?> GetAsync(Guid projectId) =>
            await context.SyncedProjects.FirstOrDefaultAsync(p => p.ProjectId == projectId);
        public async Task<bool> ExistsAsync(Guid projectId) =>
            await context.SyncedProjects.AnyAsync(p => p.ProjectId == projectId);
        public void Add(SyncedProject p)    => context.SyncedProjects.Add(p);
        public void Update(SyncedProject p) => context.SyncedProjects.Update(p);
        public void Remove(SyncedProject p) => context.SyncedProjects.Remove(p);
    }
}
