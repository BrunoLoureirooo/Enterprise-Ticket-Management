using AutoMapper;
using teams.Entities.DataTransferObjects.Members;
using teams.Entities.DataTransferObjects.Projects;
using teams.Entities.DataTransferObjects.Teams;
using teams.Entities.Models;

namespace teams.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Team, TeamSummaryDto>();
            CreateMap<Team, TeamDto>();
            CreateMap<CreateTeamDto, Team>();

            CreateMap<TeamMember, TeamMemberDto>()
                .ForMember(d => d.Nome, o => o.MapFrom(s => s.User != null ? s.User.Name : string.Empty));

            CreateMap<Project, ProjectDto>()
                .ForMember(d => d.TeamIds, o => o.MapFrom(s => s.ProjectTeams.Select(pt => pt.TeamId)));
            CreateMap<CreateProjectDto, Project>();
        }
    }
}
