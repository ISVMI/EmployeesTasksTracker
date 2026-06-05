using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Exceptions;
using Shared.Messages;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks
{
    public class ChangeTaskStatusHandler : IRequestHandler<ChangeTaskStatusCommand>
    {
        private readonly ITasksRepo _repo;
        private readonly IBus _bus;
        private readonly IDistributedCache _cache;

        public ChangeTaskStatusHandler(ITasksRepo repo, IBus bus, IDistributedCache cache)
        {
            _repo = repo;
            _bus = bus;
            _cache = cache;
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

            await _cache.RemoveAsync($"task:{request.TaskId}", cancellationToken);

            await _bus.Publish(message, cancellationToken);

            await _bus.Publish(secondMessage, cancellationToken);

        }
    }
}
