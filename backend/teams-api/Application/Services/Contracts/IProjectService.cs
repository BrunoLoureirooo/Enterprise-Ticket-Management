using teams.Entities.DataTransferObjects.Projects;

namespace teams.Application.Services.Contracts
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetAllAsync();
        Task<IEnumerable<ProjectDto>> GetByTeamAsync(Guid teamId);
        Task<ProjectDto?> GetByIdAsync(Guid id);
        Task<ProjectDto> CreateAsync(CreateProjectDto dto);
        Task<ProjectDto?> UpdateAsync(Guid id, UpdateProjectDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<ProjectDto?> LinkTeamAsync(Guid projectId, Guid teamId);
        Task<ProjectDto?> UnlinkTeamAsync(Guid projectId, Guid teamId);
    }
}
