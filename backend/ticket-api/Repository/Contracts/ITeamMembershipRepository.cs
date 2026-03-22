using ticket.Entities.Models;

namespace ticket.Repository.Contracts
{
    public interface ITeamMembershipRepository
    {
        Task<IEnumerable<TeamMembership>> GetByUserAsync(Guid userId);
        Task<TeamMembership?> GetAsync(Guid userId, Guid teamId);
        void Add(TeamMembership membership);
        void Remove(TeamMembership membership);
        void Update(TeamMembership membership);
    }
}
