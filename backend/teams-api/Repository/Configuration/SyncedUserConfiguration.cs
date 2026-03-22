using teams.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace teams.Repository.Configuration
{
    public class SyncedUserConfiguration : IEntityTypeConfiguration<SyncedUser>
    {
        public void Configure(EntityTypeBuilder<SyncedUser> builder)
        {
            builder.HasKey(u => u.UserId);
            builder.Property(u => u.Name).HasMaxLength(300);
            builder.Property(u => u.Email).HasMaxLength(300);
        }
    }
}
