using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Kotoba.Modules.Infrastructure.Data
{
    public class KotobaDbContextFactory : IDesignTimeDbContextFactory<KotobaDbContext>
    {
        public KotobaDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

            var optionsBuilder = new DbContextOptionsBuilder<KotobaDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new KotobaDbContext(optionsBuilder.Options);
        }
    }
}
