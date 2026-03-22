namespace teams.Repository.Contracts
{
    public interface IRepositoryManager
    {
        ITeamRepository Team { get; }
        IProjectRepository Project { get; }
        ISyncedUserRepository SyncedUser { get; }
        Task SaveAsync();
    }
}
