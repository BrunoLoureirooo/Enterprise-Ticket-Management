using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ticket.Entities.Models;
using ticket.Repository.Contracts;

namespace ticket.Application.Messaging
{
    public class TeamSyncConsumer(
        IConfiguration config,
        IServiceScopeFactory scopeFactory,
        ILogger<TeamSyncConsumer> logger)
        : BaseConsumer("teams.teams", "ticket.teams.teams", config, scopeFactory, logger)
    {
        protected override async Task Handle(string json, IRepositoryManager repo)
        {
            var evt = Deserialize<TeamChangedEvent>(json);
            if (evt is null) return;

            var existing = await repo.SyncedTeam.GetAsync(evt.TeamId);

            switch (evt.Action)
            {
                case "Created" when existing is null:
                    repo.SyncedTeam.Add(new SyncedTeam { TeamId = evt.TeamId, Name = evt.Name });
                    break;
                case "Updated" when existing is not null:
                    existing.Name = evt.Name;
                    repo.SyncedTeam.Update(existing);
                    break;
                case "Deleted" when existing is not null:
                    repo.SyncedTeam.Remove(existing);
                    break;
            }

            await repo.SaveAsync();
        }

        private class TeamChangedEvent
        {
            public Guid TeamId { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Action { get; set; } = string.Empty;
        }
    }
}
