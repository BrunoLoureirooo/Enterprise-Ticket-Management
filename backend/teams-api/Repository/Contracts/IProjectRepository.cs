using teams.Entities.Models;

namespace teams.Repository.Contracts
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync();
        Task<IEnumerable<Project>> GetByTeamAsync(Guid teamId);
        Task<Project?> GetByIdAsync(Guid id);
        void Create(Project project);
        void Update(Project project);
        void Delete(Project project);
        void AddTeamLink(ProjectTeam link);
        void RemoveTeamLink(ProjectTeam link);
        Task<ProjectTeam?> GetTeamLinkAsync(Guid projectId, Guid teamId);
    }
}
