using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Shared.Extensions
{
    public static class DatabaseExtensions
    {

        public static void AddDatabaseService<TContext>(this IServiceCollection services, IConfiguration configuration, string connectionString = "DefaultConnection")
            where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString(connectionString)));
        }

        public static async Task InitializeDatabaseAsync<TContext>(this IServiceProvider serviceProvider, Func<TContext, Task>? initializer = null)
            where TContext : DbContext
        {
            using var score = serviceProvider.CreateScope();
            var services = score.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<TContext>();

                if (initializer != null)
                {
                    await initializer(context);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Database initialization failure: {ex.Message}");
            }
        }
    }
}
