using AutoMapper;
using ticket.Application.Services.Contracts;
using ticket.Entities.DataTransferObjects.Tickets;
using ticket.Entities.Enums;
using ticket.Entities.Models;
using ticket.Repository.Contracts;

namespace ticket.Application.Services
{
    public class TicketService(IRepositoryManager repository, IMapper mapper) : ITicketService
    {
        public async Task<IEnumerable<TicketDto>> GetAllAsync()
        {
            var tickets = await repository.Ticket.GetAllAsync();
            return mapper.Map<IEnumerable<TicketDto>>(tickets);
        }

        public async Task<IEnumerable<TicketDto>> GetScopedAsync(Guid userId)
        {
            var memberships = await repository.TeamMembership.GetByUserAsync(userId);
            var ledTeamIds  = memberships.Where(m => m.IsLeader).Select(m => m.TeamId).ToList();

            // Team leader → all tickets in their team(s) + tickets assigned to them
            if (ledTeamIds.Count > 0)
            {
                var teamTickets    = await repository.Ticket.GetByTeamsAsync(ledTeamIds);
                var assignedToMe  = await repository.Ticket.GetByUserAsync(userId);
                var combined      = teamTickets.UnionBy(assignedToMe, t => t.Id);
                return mapper.Map<IEnumerable<TicketDto>>(combined);
            }

            // Regular member → only tickets assigned to them
            var myTickets = await repository.Ticket.GetByUserAsync(userId);
            return mapper.Map<IEnumerable<TicketDto>>(myTickets);
        }

        public async Task<IEnumerable<TicketDto>> GetByUserAsync(Guid userId)
        {
            var tickets = await repository.Ticket.GetByUserAsync(userId);
            return mapper.Map<IEnumerable<TicketDto>>(tickets);
        }

        public async Task<TicketDto?> GetByIdAsync(Guid id)
        {
            var ticket = await repository.Ticket.GetByIdAsync(id);
            return ticket is null ? null : mapper.Map<TicketDto>(ticket);
        }

        public async Task<TicketDto> CreateAsync(Guid userId, CreateTicketDto dto)
        {
            if (dto.AssignedToId.HasValue && !await repository.SyncedUser.ExistsAsync(dto.AssignedToId.Value))
                throw new InvalidOperationException("Assigned user does not exist.");

            if (dto.TeamId.HasValue && !await repository.SyncedTeam.ExistsAsync(dto.TeamId.Value))
                throw new InvalidOperationException("Team does not exist.");

            if (dto.ProjectId.HasValue && !await repository.SyncedProject.ExistsAsync(dto.ProjectId.Value))
                throw new InvalidOperationException("Project does not exist.");

            var ticket = mapper.Map<Ticket>(dto);
            ticket.CreatedById  = userId;
            ticket.AssignedToId = dto.AssignedToId ?? userId;
            ticket.TeamId       = dto.TeamId;
            ticket.ProjectId    = dto.ProjectId;
            repository.Ticket.Create(ticket);
            await repository.SaveAsync();
            return mapper.Map<TicketDto>(ticket);
        }

        public async Task<TicketDto?> UpdateAsync(Guid id, UpdateTicketDto dto)
        {
            var ticket = await repository.Ticket.GetByIdAsync(id);
            if (ticket is null) return null;

            if (dto.AssignedToId.HasValue && !await repository.SyncedUser.ExistsAsync(dto.AssignedToId.Value))
                throw new InvalidOperationException("Assigned user does not exist.");

            if (dto.Title is not null) ticket.Title = dto.Title;
            if (dto.Description is not null) ticket.Description = dto.Description;
            if (dto.AssignedToId.HasValue) ticket.AssignedToId = dto.AssignedToId;

            if (dto.Priority is not null && Enum.TryParse<TicketPriority>(dto.Priority, true, out var priority))
                ticket.Priority = priority;

            if (dto.Status is not null && Enum.TryParse<TicketStatus>(dto.Status, true, out var status))
                ticket.Status = status;

            ticket.UpdatedAt = DateTime.UtcNow;
            repository.Ticket.Update(ticket);
            await repository.SaveAsync();
            return mapper.Map<TicketDto>(ticket);
        }

        public async Task<TicketDto?> AssignAsync(Guid id, Guid assignedToId)
        {
            var ticket = await repository.Ticket.GetByIdAsync(id);
            if (ticket is null) return null;

            if (!await repository.SyncedUser.ExistsAsync(assignedToId))
                throw new InvalidOperationException("Assigned user does not exist.");

            ticket.AssignedToId = assignedToId;
            ticket.UpdatedAt = DateTime.UtcNow;
            repository.Ticket.Update(ticket);
            await repository.SaveAsync();
            return mapper.Map<TicketDto>(ticket);
        }

        public async Task<TicketDto?> CloseAsync(Guid id)
        {
            var ticket = await repository.Ticket.GetByIdAsync(id);
            if (ticket is null) return null;

            ticket.Status = TicketStatus.Closed;
            ticket.ClosedAt = DateTime.UtcNow;
            ticket.UpdatedAt = DateTime.UtcNow;
            repository.Ticket.Update(ticket);
            await repository.SaveAsync();
            return mapper.Map<TicketDto>(ticket);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var ticket = await repository.Ticket.GetByIdAsync(id);
            if (ticket is null) return false;

            repository.Ticket.Delete(ticket);
            await repository.SaveAsync();
            return true;
        }

        public async Task<TicketStatsDto> GetStatsAsync(Guid userId, bool isAdmin)
        {
            var unresolved = new[] { TicketStatus.Open, TicketStatus.InProgress };

            if (isAdmin)
            {
                var allTickets   = (await repository.Ticket.GetAllAsync()).ToList();
                var allUnresolved = allTickets.Where(t => unresolved.Contains(t.Status)).ToList();

                return new TicketStatsDto
                {
                    UnresolvedByStatus = allUnresolved
                        .GroupBy(t => t.Status.ToString())
                        .Select(g => new StatusCountDto { Status = g.Key, Count = g.Count() })
                        .ToList(),
                    ByPriority = allTickets
                        .GroupBy(t => t.Priority.ToString())
                        .Select(g => new PriorityCountDto { Priority = g.Key, Count = g.Count() })
                        .ToList(),
                    TeamUnresolvedByPriority = allUnresolved
                        .GroupBy(t => t.Priority.ToString())
                        .Select(g => new PriorityCountDto { Priority = g.Key, Count = g.Count() })
                        .ToList(),
                    IsTeamLeader = false,
                    IsAdmin      = true,
                };
            }

            var memberships  = await repository.TeamMembership.GetByUserAsync(userId);
            var ledTeamIds   = memberships.Where(m => m.IsLeader).Select(m => m.TeamId).ToList();
            var isTeamLeader = ledTeamIds.Count > 0;

            var myTickets    = (await repository.Ticket.GetByUserAsync(userId)).ToList();
            var myUnresolved = myTickets.Where(t => unresolved.Contains(t.Status)).ToList();

            List<PriorityCountDto>? teamUnresolvedByPriority = null;
            if (isTeamLeader)
            {
                var teamTickets = (await repository.Ticket.GetByTeamsAsync(ledTeamIds)).ToList();
                teamUnresolvedByPriority = teamTickets
                    .Where(t => unresolved.Contains(t.Status))
                    .GroupBy(t => t.Priority.ToString())
                    .Select(g => new PriorityCountDto { Priority = g.Key, Count = g.Count() })
                    .ToList();
            }

            return new TicketStatsDto
            {
                UnresolvedByStatus = myUnresolved
                    .GroupBy(t => t.Status.ToString())
                    .Select(g => new StatusCountDto { Status = g.Key, Count = g.Count() })
                    .ToList(),
                ByPriority = myTickets
                    .GroupBy(t => t.Priority.ToString())
                    .Select(g => new PriorityCountDto { Priority = g.Key, Count = g.Count() })
                    .ToList(),
                TeamUnresolvedByPriority = teamUnresolvedByPriority,
                IsTeamLeader             = isTeamLeader,
                IsAdmin                  = false,
            };
        }
    }
}
