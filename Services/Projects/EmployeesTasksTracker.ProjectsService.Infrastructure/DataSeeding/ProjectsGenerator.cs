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
                    Description = $"Проект позволяет {_faker.Hacker.Verb()} {_faker.Hacker.Noun()} и {_faker.Hacker.Verb()} {_faker.Hacker.Noun()}",
                    Supervisor = supervisor,
                    Manager = manager
                };

                projects.Add(project);
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
