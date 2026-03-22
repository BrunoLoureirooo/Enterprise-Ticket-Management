using ticket.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ticket.Repository.Configuration
{
    public class TeamMembershipConfiguration : IEntityTypeConfiguration<TeamMembership>
    {
        public void Configure(EntityTypeBuilder<TeamMembership> builder)
        {
            builder.HasKey(m => new { m.UserId, m.TeamId });
        }
    }
}
