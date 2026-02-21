using AutoMapper;
using backend.Entities.DataTransferObjects;
using backend.Entities.Models;

namespace backend.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForRegistrationDto, ApplicationUser>()
                .ForMember(dest => dest.Nome,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}".Trim()));
        }
    }
}
