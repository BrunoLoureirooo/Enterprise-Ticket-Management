using AutoMapper;
using ticket.Application.Services;
using ticket.Application.Services.Contracts;
using ticket.Repository.Contracts;

namespace ticket.Application
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<ITicketService> _ticketService;

        public ServiceManager(IRepositoryManager repository, IMapper mapper)
        {
            _ticketService = new Lazy<ITicketService>(() => new TicketService(repository, mapper));
        }

        public ITicketService TicketService => _ticketService.Value;
    }
}
