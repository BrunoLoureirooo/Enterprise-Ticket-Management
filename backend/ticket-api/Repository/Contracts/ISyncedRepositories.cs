using ticket.Entities.Models;

namespace ticket.Repository.Contracts
{
    public interface ISyncedUserRepository
    {
        Task<SyncedUser?> GetAsync(Guid userId);
        Task<bool> ExistsAsync(Guid userId);
        void Add(SyncedUser user);
        void Update(SyncedUser user);
        void Remove(SyncedUser user);
    }

    public interface ISyncedTeamRepository
    {
        Task<SyncedTeam?> GetAsync(Guid teamId);
        Task<bool> ExistsAsync(Guid teamId);
        void Add(SyncedTeam team);
        void Update(SyncedTeam team);
        void Remove(SyncedTeam team);
    }

    public interface ISyncedProjectRepository
    {
        Task<SyncedProject?> GetAsync(Guid projectId);
        Task<bool> ExistsAsync(Guid projectId);
        void Add(SyncedProject project);
        void Update(SyncedProject project);
        void Remove(SyncedProject project);
    }
}
