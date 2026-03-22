using backend.Application.Services.Contracts;
using backend.Repository;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace backend.Application.ClaimEnrichers
{
    public class TeamLeaderClaimEnricher : IClaimEnricher
    {
        private readonly RepositoryContext _context;

        public TeamLeaderClaimEnricher(RepositoryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Claim>> EnrichAsync(Guid userId)
        {
            var isLeader = await _context.SyncedTeamMemberships
                .AnyAsync(m => m.UserId == userId && m.IsLeader);

            return new[] { new Claim("is_team_leader", isLeader ? "true" : "false") };
        }
    }
}
