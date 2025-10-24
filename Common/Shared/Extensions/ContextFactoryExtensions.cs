using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Shared.Extensions
{
    public static class ContextFactoryExtensions
    {
        public static DbContextOptionsBuilder<TContext> GetOptionsBuilder<TContext>()
            where TContext : DbContext
        {
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            var path = Directory.GetCurrentDirectory()[..^14] + "Api";

            var configuration = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            return optionsBuilder;
        }
    }
}
