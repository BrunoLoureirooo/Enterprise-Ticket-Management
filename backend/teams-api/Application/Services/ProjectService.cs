using AutoMapper;
using teams.Application.Messaging;
using teams.Application.Services.Contracts;
using teams.Entities.DataTransferObjects.Projects;
using teams.Entities.Events;
using teams.Entities.Models;
using teams.Repository.Contracts;

namespace teams.Application.Services
{
    public class ProjectService(IRepositoryManager repository, IMapper mapper, RabbitMqPublisher publisher) : IProjectService
    {
        public async Task<IEnumerable<ProjectDto>> GetAllAsync()
        {
            var projects = await repository.Project.GetAllAsync();
            return mapper.Map<IEnumerable<ProjectDto>>(projects);
        }

        public async Task<IEnumerable<ProjectDto>> GetByTeamAsync(Guid teamId)
        {
            var projects = await repository.Project.GetByTeamAsync(teamId);
            return mapper.Map<IEnumerable<ProjectDto>>(projects);
        }

        public async Task<ProjectDto?> GetByIdAsync(Guid id)
        {
            var project = await repository.Project.GetByIdAsync(id);
            return project is null ? null : mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto> CreateAsync(CreateProjectDto dto)
        {
            foreach (var teamId in dto.TeamIds.Distinct())
                if (await repository.Team.GetByIdAsync(teamId) is null)
                    throw new InvalidOperationException($"Team {teamId} does not exist.");

            var project = mapper.Map<Project>(dto);
            repository.Project.Create(project);

            foreach (var teamId in dto.TeamIds.Distinct())
                repository.Project.AddTeamLink(new ProjectTeam { ProjectId = project.Id, TeamId = teamId });

            await repository.SaveAsync();
            var created = await repository.Project.GetByIdAsync(project.Id);
            publisher.PublishProject(new ProjectChangedEvent
            {
                ProjectId = project.Id,
                Name      = project.Name,
                TeamIds   = created!.ProjectTeams.Select(pt => pt.TeamId).ToList(),
                Action    = "Created",
            });
            return mapper.Map<ProjectDto>(created);
        }

        public async Task<ProjectDto?> UpdateAsync(Guid id, UpdateProjectDto dto)
        {
            var project = await repository.Project.GetByIdAsync(id);
            if (project is null) return null;

            if (dto.Name is not null) project.Name = dto.Name;
            if (dto.Description is not null) project.Description = dto.Description;
            project.UpdatedAt = DateTime.UtcNow;

            repository.Project.Update(project);
            await repository.SaveAsync();
            publisher.PublishProject(new ProjectChangedEvent
            {
                ProjectId = project.Id,
                Name      = project.Name,
                TeamIds   = project.ProjectTeams.Select(pt => pt.TeamId).ToList(),
                Action    = "Updated",
            });
            return mapper.Map<ProjectDto>(project);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var project = await repository.Project.GetByIdAsync(id);
            if (project is null) return false;
            publisher.PublishProject(new ProjectChangedEvent
            {
                ProjectId = project.Id,
                Name      = project.Name,
                TeamIds   = new(),
                Action    = "Deleted",
            });
            repository.Project.Delete(project);
            await repository.SaveAsync();
            return true;
        }

        public async Task<ProjectDto?> LinkTeamAsync(Guid projectId, Guid teamId)
        {
            var project = await repository.Project.GetByIdAsync(projectId);
            if (project is null) return null;

            if (await repository.Team.GetByIdAsync(teamId) is null)
                throw new InvalidOperationException("Team does not exist.");

            var existing = await repository.Project.GetTeamLinkAsync(projectId, teamId);
            if (existing is null)
            {
                repository.Project.AddTeamLink(new ProjectTeam { ProjectId = projectId, TeamId = teamId });
                await repository.SaveAsync();
            }

            return mapper.Map<ProjectDto>(await repository.Project.GetByIdAsync(projectId));
        }

        public async Task<ProjectDto?> UnlinkTeamAsync(Guid projectId, Guid teamId)
        {
            var link = await repository.Project.GetTeamLinkAsync(projectId, teamId);
            if (link is null) return null;

            repository.Project.RemoveTeamLink(link);
            await repository.SaveAsync();
            return mapper.Map<ProjectDto>(await repository.Project.GetByIdAsync(projectId));
        }
    }
}
