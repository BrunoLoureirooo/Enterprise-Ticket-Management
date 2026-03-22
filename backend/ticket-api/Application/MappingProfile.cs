using AutoMapper;
using ticket.Entities.DataTransferObjects.Tickets;
using ticket.Entities.Models;

namespace ticket.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Ticket, TicketDto>()
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.Priority, o => o.MapFrom(s => s.Priority.ToString()));

            CreateMap<CreateTicketDto, Ticket>();
        }
    }
}
