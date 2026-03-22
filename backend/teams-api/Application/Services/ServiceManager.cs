using AutoMapper;
using teams.Application.Messaging;
using teams.Application.Services.Contracts;
using teams.Repository.Contracts;

namespace teams.Application.Services
{
    public class ServiceManager(IRepositoryManager repository, IMapper mapper, RabbitMqPublisher publisher) : IServiceManager
    {
        private ITeamService? _team;
        private IProjectService? _project;

        public ITeamService TeamService     => _team    ??= new TeamService(repository, mapper, publisher);
        public IProjectService ProjectService => _project ??= new ProjectService(repository, mapper, publisher);
    }
}
