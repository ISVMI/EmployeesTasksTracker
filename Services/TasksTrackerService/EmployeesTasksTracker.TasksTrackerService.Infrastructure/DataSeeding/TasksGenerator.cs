using Bogus;
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using Npgsql;

namespace EmployeesTasksTracker.TasksTrackerService.Infrastructure.DataSeeding
{
    public class TasksGenerator
    {
        private static readonly Faker _faker = new("ru");
        private readonly IEmployeesRepo _employeesRepo;
        private readonly ITasksGroupsRepo _tasksGroupsRepo;
        private readonly IProjectsRepo _projectsRepo;
        private static readonly Random _random = new Random();

        public TasksGenerator(
            IEmployeesRepo employeesRepo,
            ITasksGroupsRepo tasksGroupsRepo,
            IProjectsRepo projectsRepo)
        {
            _employeesRepo = employeesRepo;
            _tasksGroupsRepo = tasksGroupsRepo;
            _projectsRepo = projectsRepo;
        }

        public async System.Threading.Tasks.Task GenerateTasksAsync(int count, int batchSize, string connectionString)
        {
            var employees = await _employeesRepo.GetAllIds();
            var tasksGroups = await _tasksGroupsRepo.GetAllIds();
            var projects = await _projectsRepo.GetAllIds();

            if (!employees.Any())
            {
                throw new ArgumentNullException(nameof(employees), "There were no employees!");
            }

            if (!tasksGroups.Any())
            {
                throw new ArgumentNullException(nameof(tasksGroups), "There were no tasks groups!");
            }

            if (!projects.Any())
            {
                throw new ArgumentNullException(nameof(projects), "There were no projects!");
            }

            var employeesList = employees.ToList();
            var tasksGroupsList = tasksGroups.ToList();
            var projectsList = projects.ToList();

            Shuffle(employeesList);
            Shuffle(tasksGroupsList);
            Shuffle(projectsList);

            var employeesShuffled = new Queue<Guid>(employeesList);
            var tasksGroupsShuffled = new Queue<Guid>(tasksGroupsList);
            var projectsShuffled = new Queue<Guid>(projectsList);

            var tasksGroup = tasksGroupsShuffled.Dequeue();
            var project = projectsShuffled.Dequeue();

            var tasksBatch = new List<Core.Models.Task>(batchSize);
            var tasksEmployeesBatch = new List<TaskEmployee>(batchSize * 3);

            for (int i = 0; i < count; i++)
            {

                if (employeesShuffled.Count == 0)
                {
                    employeesList.ForEach(employeesShuffled.Enqueue);
                }

                if (tasksGroupsShuffled.Count == 0)
                {
                    tasksGroupsList.ForEach(tasksGroupsShuffled.Enqueue);
                }

                var performers = GetFewEmployees(_random.Next(1, 5), employeesShuffled);
                var observers = GetFewEmployees(_random.Next(1, 2), employeesShuffled);

                if (projectsShuffled.Count == 0)
                {
                    projectsList.ForEach(projectsShuffled.Enqueue);
                }

                if (i % 3 == 0)
                {
                    tasksGroup = tasksGroupsShuffled.Dequeue();
                }

                if (i % 6 == 0)
                {
                    project = projectsShuffled.Dequeue();
                }

                var name = _faker.Hacker.Verb();
                var capitalizedVerb = char.ToUpper(name[0]) + name[1..];

                var task = new Core.Models.Task
                {
                    Name = $"{capitalizedVerb} {_faker.Hacker.Adjective()} {_faker.Hacker.Noun()}",
                    Description = $"Необходимо {_faker.Hacker.Verb()} {_faker.Hacker.Noun()} и {_faker.Hacker.Verb()} {_faker.Hacker.Noun()}",

                    ProjectId = project,
                    TasksGroupId = tasksGroup,
                    Deadline = DateTime.UtcNow + TimeSpan.FromDays(_faker.Random.Double(6, 366)),
                    Priority = _faker.PickRandom<Priority>()
                };

                tasksBatch.Add(task);

                foreach (var performer in performers)
                {
                    tasksEmployeesBatch.Add(new TaskEmployee
                    {
                        TaskId = task.Id,
                        EmployeeId = performer,
                        EmployeeRoleInTask = RoleInTask.Performer
                    });
                }

                foreach (var observer in observers)
                {
                    tasksEmployeesBatch.Add(new TaskEmployee
                    {
                        TaskId = task.Id,
                        EmployeeId = observer,
                        EmployeeRoleInTask = RoleInTask.Observer
                    });
                }

                if (tasksBatch.Count >= batchSize)
                {
                    await InsertTasks(tasksBatch, connectionString);
                    await InsertTasksEmployees(tasksEmployeesBatch, connectionString);

                    tasksBatch.Clear();
                    tasksEmployeesBatch.Clear();
                }
            }

            if (tasksBatch.Count > 0)
            {
                await InsertTasks(tasksBatch, connectionString);
                await InsertTasksEmployees(tasksEmployeesBatch, connectionString);
            }

        }

        private static void Shuffle(List<Guid> employees)
        {

            for (int i = employees.Count - 1; i > 0; i--)
            {
                int j = _random.Next(0, i);

                (employees[i], employees[j]) = (employees[j], employees[i]);
            }
        }

        private static List<Guid> GetFewEmployees(int quantity, Queue<Guid> employees)
        {
            var employeesPart = new List<Guid>();

            for (int i = 0; i < quantity && employees.Count > 0; i++)
            {
                employeesPart.Add(employees.Dequeue());
            }

            return employeesPart;
        }

        private static async System.Threading.Tasks.Task InsertTasks(List<Core.Models.Task> tasks, string connectionString)
        {
            await using var connection = new NpgsqlConnection(connectionString);

            await connection.OpenAsync();

            await using var writer = connection.BeginBinaryImport(@" COPY ""Tasks"" 
                    (""Id"", ""Name"", ""Description"", ""ProjectId"", ""TasksGroupId"", ""Deadline"", ""CreatedAt"", ""Status"", ""Priority"")
                    FROM STDIN (FORMAT BINARY)");

            foreach (var task in tasks)
            {
                writer.StartRow();
                writer.Write(task.Id);
                writer.Write(task.Name);
                writer.Write(task.Description);
                writer.Write(task.ProjectId);
                writer.Write(task.TasksGroupId);
                writer.Write(task.Deadline);
                writer.Write(task.CreatedAt);
                writer.Write((int)task.Status);
                writer.Write((int)task.Priority);
            }
            await writer.CompleteAsync();
        }

        private static async System.Threading.Tasks.Task InsertTasksEmployees(List<TaskEmployee> tasksEmployees, string connectionString)
        {

            await using var connection = new NpgsqlConnection(connectionString);

            await connection.OpenAsync();

            await using var writer = connection.BeginBinaryImport(@" COPY ""TaskEmployees"" 
                    (""TaskId"", ""EmployeeId"", ""EmployeeRoleInTask"")
                    FROM STDIN (FORMAT BINARY)");

            foreach (var taskEmployee in tasksEmployees)
            {
                writer.StartRow();
                writer.Write(taskEmployee.TaskId);
                writer.Write(taskEmployee.EmployeeId);
                writer.Write((int)taskEmployee.EmployeeRoleInTask);
            }
            await writer.CompleteAsync();
        }
    }
}