using Bogus;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;

namespace EmployeesTasksTracker.TasksTrackerService.Infrastructure.DataSeeding
{
    public class ProjectsGenerator
    {
        private static readonly Faker _faker = new("ru");
        private readonly IEmployeesRepo _employeesRepo;
        private readonly IProjectEmployeeRepo _projectEmployeesRepo;

        public ProjectsGenerator(IEmployeesRepo employeesRepo, IProjectEmployeeRepo projectEmployeeRepo)
        {
            _employeesRepo = employeesRepo;
            _projectEmployeesRepo = projectEmployeeRepo;
        }

        public async Task<List<Project>> GenerateProjectsAsync(int count)
        {
            var projects = new List<Project>();
            var employees = await _employeesRepo.GetAllIds();

            if (employees == null)
            {
                throw new ArgumentNullException(nameof(employees), "There were no employees!");
            }

            var employeesList = employees.ToList();

            Shuffle(employeesList);

            var employeesShuffled = new Queue<Guid>(employeesList);

            for (int i = 0; i < count; i++)
            {
                var manager = employeesShuffled.Dequeue();
                var supervisor = employeesShuffled.Dequeue();
                var name = _faker.Hacker.Adjective();
                var capitalizedName = char.ToUpper(name[0]) + name[1..];

                var project = new Project
                {
                    Name = $"{capitalizedName} {_faker.Hacker.Noun()}",
                    Description = $"Проект позволяет {_faker.Hacker.Verb()} {_faker.Hacker.Noun()} и {_faker.Hacker.Verb()} {_faker.Hacker.Noun()}"
                };

                projects.Add(project);

                await _projectEmployeesRepo.AddEmployeeAsync(supervisor, project.Id, Core.Enums.RoleInProject.Supervisor);
                await _projectEmployeesRepo.AddEmployeeAsync(manager, project.Id, Core.Enums.RoleInProject.Manager);
            }

            return projects;
        }

        private static void Shuffle(List<Guid> employees)
        {
            var random = new Random();

            for (int i = employees.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i - 1);

                (employees[i], employees[j]) = (employees[j], employees[i]);
            }
        }
    }
}
