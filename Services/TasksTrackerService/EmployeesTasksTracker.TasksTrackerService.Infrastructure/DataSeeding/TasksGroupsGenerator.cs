using Bogus;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using Npgsql;
using Shared.Methods;

namespace EmployeesTasksTracker.TasksTrackerService.Infrastructure.DataSeeding
{
    public static class TasksGroupsGenerator
    {
        private static readonly Faker _faker = new("ru");

        public static async System.Threading.Tasks.Task GenerateTasksGroupsAsync(int count, int batchSize, string connectionString)
        {
            var tasksGroups = BatchesGenerator.GenerateBatches<TasksGroup>(count, batchSize, GenerateTasksGroup);

            await Parallel.ForEachAsync(tasksGroups, new ParallelOptions { MaxDegreeOfParallelism = 4 }, async (batch, _) =>
            {
                await using var connection = new NpgsqlConnection(connectionString);

                await connection.OpenAsync();

                await using var writer = connection.BeginBinaryImport(@" COPY ""TasksGroups"" 
                                    (""Id"", ""Name"")
                                    FROM STDIN (FORMAT BINARY)");

                foreach (var tasksGroup in batch)
                {
                    writer.StartRow();
                    writer.Write(tasksGroup.Id);
                    writer.Write(tasksGroup.Name);
                }

                await writer.CompleteAsync();
            });
        }

        private static TasksGroup GenerateTasksGroup()
        {

            var actions = new [] {"Внедрить", "Реализовать", "Разработать", "Создать", "Спроектировать"};

                var tasksGroup = new TasksGroup
                {
                    Name = $"{_faker.PickRandom(actions)} {_faker.Hacker.Adjective()} {_faker.Hacker.Noun()}"
                };

            return tasksGroup;
        }

        
    }
}
