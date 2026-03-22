using ticket.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ticket.Repository.Configuration
{
    public class SyncedProjectConfiguration : IEntityTypeConfiguration<SyncedProject>
    {
        public void Configure(EntityTypeBuilder<SyncedProject> builder)
        {
            builder.HasKey(p => p.ProjectId);
            builder.Property(p => p.Name).HasMaxLength(200);
        }
    }
}
