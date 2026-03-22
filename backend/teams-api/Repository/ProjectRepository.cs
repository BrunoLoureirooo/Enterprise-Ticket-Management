using teams.Entities.Models;
using teams.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace teams.Repository
{
    public class ProjectRepository(RepositoryContext context) : IProjectRepository
    {
        public async Task<IEnumerable<Project>> GetAllAsync() =>
            await context.Projects.Include(p => p.ProjectTeams).OrderBy(p => p.Name).ToListAsync();

        public async Task<IEnumerable<Project>> GetByTeamAsync(Guid teamId) =>
            await context.Projects
                .Include(p => p.ProjectTeams)
                .Where(p => p.ProjectTeams.Any(pt => pt.TeamId == teamId))
                .OrderBy(p => p.Name)
                .ToListAsync();

        public async Task<Project?> GetByIdAsync(Guid id) =>
            await context.Projects.Include(p => p.ProjectTeams).FirstOrDefaultAsync(p => p.Id == id);

        public void Create(Project project) => context.Projects.Add(project);
        public void Update(Project project) => context.Projects.Update(project);
        public void Delete(Project project) => context.Projects.Remove(project);
        public void AddTeamLink(ProjectTeam link) => context.ProjectTeams.Add(link);
        public void RemoveTeamLink(ProjectTeam link) => context.ProjectTeams.Remove(link);

        public async Task<ProjectTeam?> GetTeamLinkAsync(Guid projectId, Guid teamId) =>
            await context.ProjectTeams.FirstOrDefaultAsync(pt => pt.ProjectId == projectId && pt.TeamId == teamId);
    }
}
