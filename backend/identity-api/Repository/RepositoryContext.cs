using backend.Entities.Models;
using backend.Repository.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class RepositoryContext(DbContextOptions<RepositoryContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
    {
        public DbSet<SyncedTeamMembership> SyncedTeamMemberships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new SyncedTeamMembershipConfiguration());
        }
    }
}