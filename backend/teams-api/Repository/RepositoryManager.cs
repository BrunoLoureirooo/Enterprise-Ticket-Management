using teams.Repository.Contracts;

namespace teams.Repository
{
    public class RepositoryManager(RepositoryContext context) : IRepositoryManager
    {
        private ITeamRepository? _team;
        private IProjectRepository? _project;
        private ISyncedUserRepository? _syncedUser;

        public ITeamRepository Team => _team ??= new TeamRepository(context);
        public IProjectRepository Project => _project ??= new ProjectRepository(context);
        public ISyncedUserRepository SyncedUser => _syncedUser ??= new SyncedUserRepository(context);

        public async Task SaveAsync() => await context.SaveChangesAsync();
    }
}
