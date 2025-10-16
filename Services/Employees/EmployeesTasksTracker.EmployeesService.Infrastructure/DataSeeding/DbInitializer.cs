using EmployeesTasksTracker.EmployeesService.Infrastructure.Data;

namespace EmployeesTasksTracker.EmployeesService.Infrastructure.DataSeeding
{
    public class DbInitializer
    {
        public static async Task InitializeAsync(EmployeesContext context)
        {
            if (context.Employees.Any())
            {
                return;
            }

            var employees = await EmployeesGenerator.GenerateEmployeesAsync(20);

            if (!employees.Any()) 
            {
                return;
            }

            await context.Employees.AddRangeAsync(employees);
            await context.SaveChangesAsync();
        }
    }
}
