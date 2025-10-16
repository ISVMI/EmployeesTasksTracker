using EmployeesTasksTracker.EmployeesService.Application.Handlers;
using EmployeesTasksTracker.EmployeesService.Application.Mapping;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EmployeesTasksTracker.EmployeesService.Application.Extensions
{
    public static class ServicesRegistration
    {

        public static IServiceCollection AddApplication(this IServiceCollection services) 
        {
            services.AddAutoMapper(cfg => cfg.AddProfile<EmployeesProfile>(), Assembly.GetExecutingAssembly());

            var assemblies = new Assembly[]
            {
                Assembly.GetExecutingAssembly(),
                typeof(CreateEmployeeHandler).Assembly,
                typeof(EditEmployeeHandler).Assembly,
                typeof(DeleteEmployeeHandler).Assembly,
                typeof(GetAllEmployeesHandler).Assembly,
                typeof(GetEmployeeByIdHandler).Assembly,
            };

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

            return services;
        }
    }
}
