using EmployeesTasksTracker.NotificationService.Hubs;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Shared.Messages;

namespace EmployeesTasksTracker.NotificationService.Consumers
{
    public class TaskStatusChangedConsumer : IConsumer<TaskStatusChanged>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public TaskStatusChangedConsumer(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<TaskStatusChanged> context)
        {

            Console.WriteLine("Sending notification about task status...");

            await _hubContext.Clients.All.SendAsync("ReceiveNotification", new
            {
                type = "TaskStatusChanged",
                taskId = context.Message.TaskId,
                taskName = context.Message.TaskName,
                oldStatus = context.Message.OldStatus,
                newStatus = context.Message.NewStatus,
                time = DateTime.Now
            });
        }
    }
}
