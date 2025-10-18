using Bogus;
using EmployeesTasksTracker.ProjectsService.Application.Interfaces;
using EmployeesTasksTracker.ProjectsService.Core.Models;

namespace EmployeesTasksTracker.ProjectsService.Infrastructure.DataSeeding
{
    public class ProjectsGenerator
    {
        private static readonly Faker _faker = new("ru");
        private readonly IEmployeesClient _employeesClient;

        public ProjectsGenerator(IEmployeesClient employeesClient)
        {
            _employeesClient = employeesClient;
        }

        public async Task<List<Project>> GenerateProjectsAsync(int count)
        {
            var projects = new List<Project>();
            var employees = await _employeesClient.GetAllEmployeesIds();

            if (employees == null) 
            {
                throw new ArgumentNullException(nameof(employees), "There were no employees!");
            }

            var employeesList = employees.ToList();

            for (int i = 0; i < count; i++)
            {
                var manager = GetEmployees(employeesList);
                var supervisor = GetEmployees(employeesList);
                var project = new Project
                {
                    Name = _faker.Name.JobTitle(),
                    Description = _faker.Name.JobDescriptor(),
                    Supervisor = supervisor,
                    Manager = manager
                };

                projects.Add(project);
            }

            return projects;
        }

        private Guid GetEmployees(List<Guid> employees)
        {
            var employee = _faker.PickRandom(employees);
            employees.Remove(employee);
            return employee;
        }
    }
}
