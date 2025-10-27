using EmployeesTasksTracker.TasksService.Application.Commands;
using EmployeesTasksTracker.TasksService.Core.Interfaces;
using MassTransit;
using MediatR;
using Shared.Messages;

namespace EmployeesTasksTracker.TasksService.Application.Handlers
{
    public class ChangeTaskStatusHandler : IRequestHandler<ChangeTaskStatusCommand>
    {
        private readonly ITasksRepo _repo;
        private readonly IBus _bus;

        public ChangeTaskStatusHandler(ITasksRepo repo, IBus bus)
        {
            _repo = repo;
            _bus = bus;
        }

        public async Task Handle(ChangeTaskStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingTask = await _repo.GetByIdAsync(request.TaskId);

                await _repo.ChangeStatusAsync(request.TaskId, request.NewStatus, cancellationToken);

                var changes = new List<string>
                {
                    $"Статус изменился с {existingTask.Status} на {request.NewStatus}" 
                };

                var message = new TaskDataChanged(request.TaskId, changes, DateTime.UtcNow);

                await _bus.Publish(message, cancellationToken);
            }
            catch (Exception ex) 
            {
                throw new Exception($"Could not change task's status: {ex.Message}");
            }
        }
    }
}
