using EmployeesTasksTracker.NotificationService.Hubs;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Shared.Messages;

namespace EmployeesTasksTracker.NotificationService.Consumers
{
    public class EmployeeAssignedConsumer : IConsumer<EmployeeAssigned>
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public EmployeeAssignedConsumer(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<EmployeeAssigned> context)
        {
            Console.WriteLine("Sending notification about employee...");

            await _hubContext.Clients.All.SendAsync("ReceiveNotification", new
            {
                type = "EmployeeAssigned",
                taskId = context.Message.TaskId,
                taskName = context.Message.TaskName,
                employeeId = context.Message.EmployeeId,
                time = DateTime.Now
            });
        }
    }
}
