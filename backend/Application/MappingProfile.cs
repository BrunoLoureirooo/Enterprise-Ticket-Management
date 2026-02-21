using AutoMapper;
using backend.Entities.DataTransferObjects.Users;
using backend.Entities.Models;

namespace backend.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Users
            CreateMap<UserForRegistrationDto, ApplicationUser>()
                .ForMember(dest => dest.Nome,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}".Trim()));
            
            CreateMap<ApplicationUser, UserDto>();
        }
    }
}
