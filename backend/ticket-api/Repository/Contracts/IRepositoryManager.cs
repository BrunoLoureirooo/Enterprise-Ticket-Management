namespace ticket.Repository.Contracts
{
    public interface IRepositoryManager
    {
        ITicketRepository Ticket { get; }
        ITeamMembershipRepository TeamMembership { get; }
        ISyncedUserRepository SyncedUser { get; }
        ISyncedTeamRepository SyncedTeam { get; }
        ISyncedProjectRepository SyncedProject { get; }
        Task SaveAsync();
    }
}
