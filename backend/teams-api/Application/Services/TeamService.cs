using AutoMapper;
using teams.Application.Messaging;
using teams.Application.Services.Contracts;
using teams.Entities.DataTransferObjects.Members;
using teams.Entities.DataTransferObjects.Teams;
using teams.Entities.Events;
using teams.Entities.Models;
using teams.Repository.Contracts;

namespace teams.Application.Services
{
    public class TeamService(IRepositoryManager repository, IMapper mapper, RabbitMqPublisher publisher) : ITeamService
    {
        public async Task<IEnumerable<TeamSummaryDto>> GetAllAsync()
        {
            var teams = await repository.Team.GetAllAsync();
            return mapper.Map<IEnumerable<TeamSummaryDto>>(teams);
        }

        public async Task<TeamDto?> GetByIdAsync(Guid id)
        {
            var team = await repository.Team.GetByIdAsync(id);
            return team is null ? null : mapper.Map<TeamDto>(team);
        }

        public async Task<TeamDto> CreateAsync(CreateTeamDto dto)
        {
            var team = mapper.Map<Team>(dto);
            repository.Team.Create(team);
            await repository.SaveAsync();
            publisher.PublishTeam(new TeamChangedEvent { TeamId = team.Id, Name = team.Name, Action = "Created" });
            return mapper.Map<TeamDto>(team);
        }

        public async Task<TeamDto?> UpdateAsync(Guid id, UpdateTeamDto dto)
        {
            var team = await repository.Team.GetByIdAsync(id);
            if (team is null) return null;

            if (dto.Name is not null) team.Name = dto.Name;
            if (dto.Description is not null) team.Description = dto.Description;
            team.UpdatedAt = DateTime.UtcNow;

            repository.Team.Update(team);
            await repository.SaveAsync();
            publisher.PublishTeam(new TeamChangedEvent { TeamId = team.Id, Name = team.Name, Action = "Updated" });
            return mapper.Map<TeamDto>(team);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var team = await repository.Team.GetByIdAsync(id);
            if (team is null) return false;
            publisher.PublishTeam(new TeamChangedEvent { TeamId = team.Id, Name = team.Name, Action = "Deleted" });
            repository.Team.Delete(team);
            await repository.SaveAsync();
            return true;
        }

        public async Task<TeamDto?> AddMemberAsync(Guid teamId, AddMemberDto dto)
        {
            var team = await repository.Team.GetByIdAsync(teamId);
            if (team is null) return null;

            if (!await repository.SyncedUser.ExistsAsync(dto.UserId))
                throw new InvalidOperationException("User does not exist.");

            var existing = await repository.Team.GetMemberAsync(teamId, dto.UserId);
            if (existing is not null)
            {
                existing.IsLeader = dto.IsLeader;
                repository.Team.UpdateMember(existing);
            }
            else
            {
                repository.Team.AddMember(new TeamMember
                {
                    TeamId   = teamId,
                    UserId   = dto.UserId,
                    IsLeader = dto.IsLeader,
                });
            }

            await repository.SaveAsync();

            publisher.Publish(new MembershipChangedEvent
            {
                UserId   = dto.UserId,
                TeamId   = teamId,
                IsLeader = dto.IsLeader,
                Action   = existing is null ? "Added" : "Updated",
            });

            return mapper.Map<TeamDto>(await repository.Team.GetByIdAsync(teamId));
        }

        public async Task<TeamDto?> RemoveMemberAsync(Guid teamId, Guid userId)
        {
            var member = await repository.Team.GetMemberAsync(teamId, userId);
            if (member is null) return null;

            repository.Team.RemoveMember(member);
            await repository.SaveAsync();

            publisher.Publish(new MembershipChangedEvent
            {
                UserId   = userId,
                TeamId   = teamId,
                IsLeader = false,
                Action   = "Removed",
            });

            return mapper.Map<TeamDto>(await repository.Team.GetByIdAsync(teamId));
        }

        public async Task<TeamDto?> SetLeaderAsync(Guid teamId, Guid userId, bool isLeader)
        {
            var member = await repository.Team.GetMemberAsync(teamId, userId);
            if (member is null) return null;

            member.IsLeader = isLeader;
            repository.Team.UpdateMember(member);
            await repository.SaveAsync();

            publisher.Publish(new MembershipChangedEvent
            {
                UserId   = userId,
                TeamId   = teamId,
                IsLeader = isLeader,
                Action   = "Updated",
            });

            return mapper.Map<TeamDto>(await repository.Team.GetByIdAsync(teamId));
        }
    }
}
