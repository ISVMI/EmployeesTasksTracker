using Bogus;
using EmployeesTasksTracker.TasksService.Application.Interfaces;

namespace EmployeesTasksTracker.TasksService.Infrastructure.DataSeeding
{
    public class TasksGenerator
    {
        private static readonly Faker _faker = new("ru");
        private readonly IEmployeesClient _employeesClient;
        private readonly ITasksGroupsClient _tasksGroupsClient;
        private readonly IProjectsClient _projectsClient;

        public TasksGenerator(IEmployeesClient employeesClient, ITasksGroupsClient tasksGroupsClient, IProjectsClient projectsClient)
        {
            _employeesClient = employeesClient;
            _tasksGroupsClient = tasksGroupsClient;
            _projectsClient = projectsClient;
        }

        public async Task<List<Core.Models.Task>> GenerateTasksAsync(int count)
        {
            var tasks = new List<Core.Models.Task>();
            var employees = await _employeesClient.GetAllIds();
            var tasksGroups = await _tasksGroupsClient.GetAllIds();
            var projects = await _projectsClient.GetAllIds();

            if (employees == null)
            {
                throw new ArgumentNullException(nameof(employees), "There were no employees!");
            }

            if (tasksGroups == null)
            {
                throw new ArgumentNullException(nameof(employees), "There were no tasks groups!");
            }

            if (projects == null)
            {
                throw new ArgumentNullException(nameof(employees), "There were no projects!");
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

            for (int i = 0; i < count; i++)
            {
                var performers = GetFewEmployees(_faker.Random.Int(0, 5), employeesShuffled);
                var observers = GetFewEmployees(_faker.Random.Int(0, 3), employeesShuffled);
                var tasksGroup = tasksGroupsShuffled.Dequeue();
                var project = projectsShuffled.Dequeue();
                var name = _faker.Hacker.Adjective();
                var capitalizedName = char.ToUpper(name[0]) + name[1..];

                var task = new Core.Models.Task
                {
                    Name = $"{_faker.Hacker.Verb()}{capitalizedName} {_faker.Hacker.Noun()}",
                    Description = $"Необходимо {_faker.Hacker.Verb()} {_faker.Hacker.Noun()} и {_faker.Hacker.Verb()} {_faker.Hacker.Noun()}",

                    Project = project,
                    TasksGroup = tasksGroup,
                    Deadline = DateTime.Now + TimeSpan.FromDays(_faker.Random.Double(6, 366)),
                    Status = _faker.PickRandom(Core.Enums.Status.Backlog, Core.Enums.Status.Current),
                    Priority = _faker.PickRandom<Core.Enums.Priority>(),
                    Performers = performers,
                    Observers = observers

                };

                tasks.Add(task);
            }

            return tasks;
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

        private static List<Guid> GetFewEmployees(int quantity, Queue<Guid> employees)
        {
            var employeesPart = new List<Guid>();

            for (int i = 0; i < quantity; i++)
            {
                employeesPart.Add(employees.Dequeue());
            }

            return employeesPart;
        }
    }
}
