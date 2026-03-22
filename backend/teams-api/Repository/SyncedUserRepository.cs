using teams.Entities.Models;
using teams.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace teams.Repository
{
    public class SyncedUserRepository(RepositoryContext context) : ISyncedUserRepository
    {
        public async Task<SyncedUser?> GetAsync(Guid userId) =>
            await context.SyncedUsers.FirstOrDefaultAsync(u => u.UserId == userId);

        public async Task<bool> ExistsAsync(Guid userId) =>
            await context.SyncedUsers.AnyAsync(u => u.UserId == userId);

        public void Add(SyncedUser user)    => context.SyncedUsers.Add(user);
        public void Update(SyncedUser user) => context.SyncedUsers.Update(user);
        public void Remove(SyncedUser user) => context.SyncedUsers.Remove(user);
    }
}
