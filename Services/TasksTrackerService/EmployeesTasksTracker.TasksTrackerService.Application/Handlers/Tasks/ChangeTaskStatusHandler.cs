using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MassTransit;
using MediatR;
using Shared.Exceptions;
using Shared.Messages;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks
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

            if (!Enum.TryParse<Status>(request.NewStatus, true, out Status newStatusEnum))
            {
                throw new DomainException($"Unknown status {request.NewStatus}");
            }

            var existingTask = await _repo.GetByIdAsync(request.TaskId, cancellationToken);

            var changes = new List<string>
                {
                    $"Статус изменился с {existingTask.Status} на {request.NewStatus}"
                };

            var message = new TaskDataChanged(request.TaskId, changes, DateTime.UtcNow);

            var secondMessage = new TaskStatusChanged
            {
                TaskId = request.TaskId,
                TaskName = existingTask.Name,
                OldStatus = existingTask.Status.ToString(),
                NewStatus = request.NewStatus
            };

            existingTask.ChangeStatus(newStatusEnum);

            await _repo.UpdateAsync(existingTask, cancellationToken);

            await _bus.Publish(message, cancellationToken);

            await _bus.Publish(secondMessage, cancellationToken);

        }
    }
}
