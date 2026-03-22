using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ticket.Repository
{
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();
            var appSettingsPath = Path.Combine(basePath, "appsettings.json");

            if (!File.Exists(appSettingsPath))
            {
                if (File.Exists(Path.Combine(basePath, "backend", "ticket-api", "API", "appsettings.json")))
                    basePath = Path.Combine(basePath, "backend", "ticket-api", "API");
                else if (File.Exists(Path.Combine(basePath, "..", "API", "appsettings.json")))
                    basePath = Path.Combine(basePath, "..", "API");
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<RepositoryContext>()
                .UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Repository"));

            return new RepositoryContext(builder.Options);
        }
    }
}
