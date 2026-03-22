using ticket.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ticket.Repository.Configuration
{
    public class SyncedTeamConfiguration : IEntityTypeConfiguration<SyncedTeam>
    {
        public void Configure(EntityTypeBuilder<SyncedTeam> builder)
        {
            builder.HasKey(t => t.TeamId);
            builder.Property(t => t.Name).HasMaxLength(200);
        }
    }
}
