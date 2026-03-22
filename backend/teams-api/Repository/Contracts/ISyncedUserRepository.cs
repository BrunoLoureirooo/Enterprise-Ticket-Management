using teams.Entities.Models;

namespace teams.Repository.Contracts
{
    public interface ISyncedUserRepository
    {
        Task<SyncedUser?> GetAsync(Guid userId);
        Task<bool> ExistsAsync(Guid userId);
        void Add(SyncedUser user);
        void Update(SyncedUser user);
        void Remove(SyncedUser user);
    }
}
