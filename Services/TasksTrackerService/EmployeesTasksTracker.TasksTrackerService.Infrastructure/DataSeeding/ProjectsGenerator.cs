using Bogus;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using Npgsql;

namespace EmployeesTasksTracker.TasksTrackerService.Infrastructure.DataSeeding
{
    public class ProjectsGenerator
    {
        private static readonly Faker _faker = new("ru");
        private readonly IEmployeesRepo _employeesRepo;

        public ProjectsGenerator(IEmployeesRepo employeesRepo)
        {
            _employeesRepo = employeesRepo;
        }

        public async System.Threading.Tasks.Task GenerateProjectsAsync(int count, int batchSize, string connectionString)
        {

            var projectsBatch = new List<Project>();

            var projectEmployeeBatch = new List<ProjectEmployee>();

            var employees = await _employeesRepo.GetAllIds();

            var employeesList = employees.ToList();

            Shuffle(employeesList);

            var employeesShuffled = new Queue<Guid>(employeesList);

            for (int i = 0; i < count; i++)
            {

                if (employees == null)
                {
                    throw new ArgumentNullException(nameof(employees), "There were no employees!");
                }

                var managerId = employeesShuffled.Dequeue();
                var supervisorId = employeesShuffled.Dequeue();
                var name = _faker.Hacker.Adjective();
                var capitalizedName = char.ToUpper(name[0]) + name[1..];

                var project = new Project
                {
                    Name = $"{capitalizedName} {_faker.Hacker.Noun()}",
                    Description = $"Проект позволяет {_faker.Hacker.Verb()} {_faker.Hacker.Noun()} и {_faker.Hacker.Verb()} {_faker.Hacker.Noun()}"
                };

                if (projectsBatch.Count > batchSize)
                {
                    await InsertprojectsAsync(projectsBatch, connectionString);
                    await InsertProjectEmployeesAsync(projectEmployeeBatch, connectionString);

                    projectsBatch.Clear();
                    projectEmployeeBatch.Clear();
                }

                projectsBatch.Add(project);

                var supervisor = new ProjectEmployee
                {
                    ProjectId = project.Id,
                    EmployeeId = supervisorId,
                    EmployeeRoleInProject = Core.Enums.RoleInProject.Supervisor
                };

                var manager = new ProjectEmployee
                {
                    ProjectId = project.Id,
                    EmployeeId = managerId,
                    EmployeeRoleInProject = Core.Enums.RoleInProject.Supervisor
                };

                projectEmployeeBatch.Add(supervisor);
                projectEmployeeBatch.Add(manager);

            }

            if (projectsBatch.Count > 0)
            {
                await InsertprojectsAsync(projectsBatch, connectionString);
                await InsertProjectEmployeesAsync(projectEmployeeBatch, connectionString);
            }
        }

        private static async System.Threading.Tasks.Task InsertprojectsAsync(List<Project> projectsBatch, string connectionString)
        {
            await using var connection = new NpgsqlConnection(connectionString);

            await connection.OpenAsync();

            await using var writer = connection.BeginBinaryImport(@" COPY ""Projects"" 
                                    (""Id"", ""Name"", ""Description"")
                                    FROM STDIN (FORMAT BINARY)");

            foreach (var project in projectsBatch)
            {
                writer.StartRow();
                writer.Write(project.Id);
                writer.Write(project.Name);
                writer.Write(project.Description);
            }

            await writer.CompleteAsync();
        }

        private static async System.Threading.Tasks.Task InsertProjectEmployeesAsync(List<ProjectEmployee> projectsEmployeesBatch, string connectionString)
        {
            await using var connection = new NpgsqlConnection(connectionString);

            await connection.OpenAsync();

            await using var writer = connection.BeginBinaryImport(@" COPY ""ProjectEmployees"" 
                                    (""ProjectId"", ""EmployeeId"", ""EmployeeRoleInProject"")
                                    FROM STDIN (FORMAT BINARY)");

            foreach (var projectEmployee in projectsEmployeesBatch)
            {
                writer.StartRow();
                writer.Write(projectEmployee.ProjectId);
                writer.Write(projectEmployee.EmployeeId);
                writer.Write((int)projectEmployee.EmployeeRoleInProject);
            }
            await writer.CompleteAsync();
        }

        private static void Shuffle(List<Guid> employees)
        {
            var random = new Random();

            for (int i = employees.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);

                (employees[i], employees[j]) = (employees[j], employees[i]);
            }
        }
    }
}