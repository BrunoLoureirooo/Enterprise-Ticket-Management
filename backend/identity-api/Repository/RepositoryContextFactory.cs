using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using backend.Entities.Models;

namespace backend.Repository
{
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args)
        {
            // Determine the path to the API project's appsettings.json
            var basePath = Directory.GetCurrentDirectory();
            var appSettingsPath = Path.Combine(basePath, "appsettings.json");

            // Adjust path if running from solution root (expects backend/API) or sibling project
            if (!File.Exists(appSettingsPath))
            {
                if (File.Exists(Path.Combine(basePath, "backend", "identity-api", "API", "appsettings.json")))
                {
                    basePath = Path.Combine(basePath, "backend", "identity-api", "API");
                }
                else if (File.Exists(Path.Combine(basePath, "..", "API", "appsettings.json")))
                {
                    basePath = Path.Combine(basePath, "..", "API");
                }
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
