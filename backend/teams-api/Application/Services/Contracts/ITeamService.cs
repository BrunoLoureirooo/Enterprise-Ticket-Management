using teams.Entities.DataTransferObjects.Members;
using teams.Entities.DataTransferObjects.Teams;

namespace teams.Application.Services.Contracts
{
    public interface ITeamService
    {
        Task<IEnumerable<TeamSummaryDto>> GetAllAsync();
        Task<TeamDto?> GetByIdAsync(Guid id);
        Task<TeamDto> CreateAsync(CreateTeamDto dto);
        Task<TeamDto?> UpdateAsync(Guid id, UpdateTeamDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<TeamDto?> AddMemberAsync(Guid teamId, AddMemberDto dto);
        Task<TeamDto?> RemoveMemberAsync(Guid teamId, Guid userId);
        Task<TeamDto?> SetLeaderAsync(Guid teamId, Guid userId, bool isLeader);
    }
}
