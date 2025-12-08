using Bogus;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;

namespace EmployeesTasksTracker.TasksTrackerService.Infrastructure.DataSeeding
{
    public static class TasksGroupsGenerator
    {
        private static readonly Faker _faker = new("ru");

        public static List<TasksGroup> GenerateTasksGroupsAsync(int count)
        {
            var tasksGroups = new List<TasksGroup>();

            var actions = new [] {"Внедрить", "Реализовать", "Разработать", "Создать", "Спроектировать"};

            for (int i = 0; i < count; i++)
            {
                var tasksGroup = new TasksGroup
                {
                    Name = $"{_faker.PickRandom(actions)} {_faker.Hacker.Adjective()} {_faker.Hacker.Noun()}"
                };

                tasksGroups.Add(tasksGroup);
            }

            return tasksGroups;
        }
    }
}
