using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Shared.Extensions;

namespace EmployeesTasksTracker.EmployeesService.Infrastructure.Data
{
    public class EmployeesContextFactory : IDesignTimeDbContextFactory<EmployeesContext>
    {
        public EmployeesContext CreateDbContext(string[] args)
        {
            var optionsBuilder = ContextFactoryExtensions.GetOptionsBuilder<EmployeesContext>();

            return new EmployeesContext(optionsBuilder.Options);
        }
    }
}
