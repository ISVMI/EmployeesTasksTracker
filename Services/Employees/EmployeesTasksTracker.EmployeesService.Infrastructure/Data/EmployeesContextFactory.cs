using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EmployeesTasksTracker.EmployeesService.Infrastructure.Data
{
    public class EmployeesContextFactory : IDesignTimeDbContextFactory<EmployeesContext>
    {
        public EmployeesContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EmployeesContext>();

            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            return new EmployeesContext(optionsBuilder.Options);
        }
    }
}
