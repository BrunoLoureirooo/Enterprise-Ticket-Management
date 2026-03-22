using ticket.Repository.Contracts;

namespace ticket.Repository
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private readonly Lazy<ITicketRepository> _ticket;
        private readonly Lazy<ITeamMembershipRepository> _teamMembership;
        private readonly Lazy<ISyncedUserRepository> _syncedUser;
        private readonly Lazy<ISyncedTeamRepository> _syncedTeam;
        private readonly Lazy<ISyncedProjectRepository> _syncedProject;

        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
            _ticket         = new Lazy<ITicketRepository>(() => new TicketRepository(context));
            _teamMembership = new Lazy<ITeamMembershipRepository>(() => new TeamMembershipRepository(context));
            _syncedUser     = new Lazy<ISyncedUserRepository>(() => new SyncedUserRepository(context));
            _syncedTeam     = new Lazy<ISyncedTeamRepository>(() => new SyncedTeamRepository(context));
            _syncedProject  = new Lazy<ISyncedProjectRepository>(() => new SyncedProjectRepository(context));
        }

        public ITicketRepository Ticket               => _ticket.Value;
        public ITeamMembershipRepository TeamMembership => _teamMembership.Value;
        public ISyncedUserRepository SyncedUser        => _syncedUser.Value;
        public ISyncedTeamRepository SyncedTeam        => _syncedTeam.Value;
        public ISyncedProjectRepository SyncedProject  => _syncedProject.Value;

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
