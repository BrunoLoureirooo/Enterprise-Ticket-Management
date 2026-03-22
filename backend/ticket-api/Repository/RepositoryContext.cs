using ticket.Entities.Models;
using ticket.Repository.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ticket.Repository
{
    public class RepositoryContext(DbContextOptions<RepositoryContext> options) : DbContext(options)
    {
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TeamMembership> TeamMemberships { get; set; }
        public DbSet<SyncedUser> SyncedUsers { get; set; }
        public DbSet<SyncedTeam> SyncedTeams { get; set; }
        public DbSet<SyncedProject> SyncedProjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new TicketConfiguration());
            modelBuilder.ApplyConfiguration(new TeamMembershipConfiguration());
            modelBuilder.ApplyConfiguration(new SyncedUserConfiguration());
            modelBuilder.ApplyConfiguration(new SyncedTeamConfiguration());
            modelBuilder.ApplyConfiguration(new SyncedProjectConfiguration());
        }
    }
}
