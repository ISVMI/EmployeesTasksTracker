using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Infrastructure.Data;
using EmployeesTasksTracker.TasksTrackerService.Infrastructure.DataSeeding;
using EmployeesTasksTracker.TasksTrackerService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Extensions;

namespace EmployeesTasksTracker.TasksTrackerService.Infrastructure.Extensions
{
    public static class InfrastructureRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            //Adding Database
            services.AddDatabaseService<TasksTrackerContext>(configuration);
            //Addind Repos
            services.AddScoped<IEmployeesRepo, EmployeesRepo>();
            services.AddScoped<IProjectEmployeeRepo, ProjectEmployeeRepo>();
            services.AddScoped<IProjectsRepo, ProjectsRepo>();
            services.AddScoped<ITaskEmployeeRepo, TaskEmployeeRepo>();
            services.AddScoped<ITasksGroupsRepo, TasksGroupsRepo>();
            services.AddScoped<ITasksRepo, TasksRepo>();

            if (isDevelopment)
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = configuration.GetConnectionString("Redis");
                });
            }

            services.AddHybridCache(options =>
            {
                options.DefaultEntryOptions = new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromMinutes(30),
                    LocalCacheExpiration = TimeSpan.FromMinutes(5),
                };
            });
        }

        public static async Task AddDatabaseInitialization(this IServiceProvider services)
        {

            using var scope = services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            try
            {
                var context = serviceProvider.GetRequiredService<TasksTrackerContext>();

                await context.Database.MigrateAsync();

                var initializer = serviceProvider.GetRequiredService<DbInitializer>();

                await initializer.InitializeAsync(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while seeding the database : {ex.Message}");
            }
        }
    }
}
