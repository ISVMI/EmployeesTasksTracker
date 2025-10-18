using EmployeesTasksTracker.ProjectsService.Application.Handlers;
using EmployeesTasksTracker.ProjectsService.Application.Mapping;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EmployeesTasksTracker.ProjectsService.Application.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => cfg.AddProfile<ProjectsProfile>(), Assembly.GetExecutingAssembly());

            var assemblies = new Assembly[]
            {
                Assembly.GetExecutingAssembly(),
                typeof(CreateProjectHandler).Assembly,
                typeof(EditProjectHandler).Assembly,
                typeof(DeleteProjectHandler).Assembly,
                typeof(GetAllProjectsHandler).Assembly,
                typeof(GetProjectByIdHandler).Assembly,
            };

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

            return services;
        }
    }
}
