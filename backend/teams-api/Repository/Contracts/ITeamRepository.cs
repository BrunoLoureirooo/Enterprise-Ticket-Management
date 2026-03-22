using teams.Entities.Models;

namespace teams.Repository.Contracts
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> GetAllAsync();
        Task<Team?> GetByIdAsync(Guid id);
        Task<TeamMember?> GetMemberAsync(Guid teamId, Guid userId);
        void Create(Team team);
        void Update(Team team);
        void Delete(Team team);
        void AddMember(TeamMember member);
        void RemoveMember(TeamMember member);
        void UpdateMember(TeamMember member);
    }
}
