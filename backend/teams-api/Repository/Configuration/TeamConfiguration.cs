using teams.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace teams.Repository.Configuration
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(200);
        }
    }

    public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
    {
        public void Configure(EntityTypeBuilder<TeamMember> builder)
        {
            builder.HasKey(m => new { m.TeamId, m.UserId });
            builder.HasOne(m => m.Team)
                   .WithMany(t => t.Members)
                   .HasForeignKey(m => m.TeamId);
        }
    }

    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        }
    }

    public class ProjectTeamConfiguration : IEntityTypeConfiguration<ProjectTeam>
    {
        public void Configure(EntityTypeBuilder<ProjectTeam> builder)
        {
            builder.HasKey(pt => new { pt.ProjectId, pt.TeamId });
            builder.HasOne(pt => pt.Project)
                   .WithMany(p => p.ProjectTeams)
                   .HasForeignKey(pt => pt.ProjectId);
            builder.HasOne(pt => pt.Team)
                   .WithMany(t => t.ProjectTeams)
                   .HasForeignKey(pt => pt.TeamId);
        }
    }
}
