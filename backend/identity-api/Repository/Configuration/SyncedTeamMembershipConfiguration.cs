using backend.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Repository.Configuration;

public class SyncedTeamMembershipConfiguration : IEntityTypeConfiguration<SyncedTeamMembership>
{
    public void Configure(EntityTypeBuilder<SyncedTeamMembership> builder)
    {
        builder.HasKey(m => new { m.UserId, m.TeamId });
    }
}
