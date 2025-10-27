using EmployeesTasksTracker.HistoryService.Core.Interfaces;
using EmployeesTasksTracker.HistoryService.Core.Models;
using MassTransit;
using Shared.Messages;

namespace EmployeesTasksTracker.HistoryService.Infrastructure.Consumers
{
    public class TaskDataChangedConsumer : IConsumer<TaskDataChanged>
    {
        private readonly ITaskChangesRepo _repo;

        public TaskDataChangedConsumer(ITaskChangesRepo repo)
        {
            _repo = repo;
        }

        public async Task Consume(ConsumeContext<TaskDataChanged> context)
        {
            try
            {
                Console.WriteLine($"Trying to write changes history for task with id {context.Message.TaskId} to the database...");

                var taskChanges = new TaskChanges
                {
                    TaskId = context.Message.TaskId,
                    ChangedAt = context.Message.ChangedAt,
                    Changes = context.Message.Changes.ToList()
                };

                await _repo.CreateTaskChangesRecord(taskChanges);

                Console.WriteLine("Successfully wrote changes to the database!");
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
