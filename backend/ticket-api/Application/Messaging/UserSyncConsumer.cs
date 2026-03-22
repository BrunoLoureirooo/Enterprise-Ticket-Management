using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ticket.Entities.Models;
using ticket.Repository.Contracts;

namespace ticket.Application.Messaging
{
    public class UserSyncConsumer(
        IConfiguration config,
        IServiceScopeFactory scopeFactory,
        ILogger<UserSyncConsumer> logger)
        : BaseConsumer("identity.users", "ticket.identity.users", config, scopeFactory, logger)
    {
        protected override async Task Handle(string json, IRepositoryManager repo)
        {
            var evt = Deserialize<UserChangedEvent>(json);
            if (evt is null) return;

            var existing = await repo.SyncedUser.GetAsync(evt.UserId);

            switch (evt.Action)
            {
                case "Created" when existing is null:
                    repo.SyncedUser.Add(new SyncedUser { UserId = evt.UserId, Name = evt.Name, Email = evt.Email });
                    break;
                case "Updated" when existing is not null:
                    existing.Name  = evt.Name;
                    existing.Email = evt.Email;
                    repo.SyncedUser.Update(existing);
                    break;
                case "Deleted" when existing is not null:
                    repo.SyncedUser.Remove(existing);
                    break;
            }

            await repo.SaveAsync();
        }

        private class UserChangedEvent
        {
            public Guid UserId { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Action { get; set; } = string.Empty;
        }
    }
}
