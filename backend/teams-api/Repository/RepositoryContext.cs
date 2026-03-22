using teams.Entities.Models;
using teams.Repository.Configuration;
using Microsoft.EntityFrameworkCore;

namespace teams.Repository
{
    public class RepositoryContext(DbContextOptions<RepositoryContext> options) : DbContext(options)
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTeam> ProjectTeams { get; set; }
        public DbSet<SyncedUser> SyncedUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new TeamConfiguration());
            modelBuilder.ApplyConfiguration(new TeamMemberConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectTeamConfiguration());
            modelBuilder.ApplyConfiguration(new SyncedUserConfiguration());
        }
    }
}
