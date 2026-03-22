using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ticket.Entities.Models;
using ticket.Repository.Contracts;

namespace ticket.Application.Messaging
{
    public class ProjectSyncConsumer(
        IConfiguration config,
        IServiceScopeFactory scopeFactory,
        ILogger<ProjectSyncConsumer> logger)
        : BaseConsumer("teams.projects", "ticket.teams.projects", config, scopeFactory, logger)
    {
        protected override async Task Handle(string json, IRepositoryManager repo)
        {
            var evt = Deserialize<ProjectChangedEvent>(json);
            if (evt is null) return;

            var existing = await repo.SyncedProject.GetAsync(evt.ProjectId);

            switch (evt.Action)
            {
                case "Created" when existing is null:
                    repo.SyncedProject.Add(new SyncedProject { ProjectId = evt.ProjectId, Name = evt.Name });
                    break;
                case "Updated" when existing is not null:
                    existing.Name = evt.Name;
                    repo.SyncedProject.Update(existing);
                    break;
                case "Deleted" when existing is not null:
                    repo.SyncedProject.Remove(existing);
                    break;
            }

            await repo.SaveAsync();
        }

        private class ProjectChangedEvent
        {
            public Guid ProjectId { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Action { get; set; } = string.Empty;
        }
    }
}
