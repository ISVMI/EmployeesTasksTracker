using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using Shared.Interfaces;
using Shared.Messages;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks
{
    public class ChangeTaskStatusHandler : IRequestHandler<ChangeTaskStatusCommand>
    {
        private readonly ITasksRepo _repo;
        private readonly IBus _bus;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly HybridCache _cache;
        private readonly ILogger<ChangeTaskStatusHandler> _logger;

        public ChangeTaskStatusHandler(ITasksRepo repo,
            IBus bus,
            IKafkaProducer kafkaProducer,
            HybridCache cache,
            ILogger<ChangeTaskStatusHandler> logger)
        {
            _repo = repo;
            _bus = bus;
            _kafkaProducer = kafkaProducer;
            _cache = cache;
            _logger = logger;
        }

        public async Task Handle(ChangeTaskStatusCommand request, CancellationToken cancellationToken)
        {

            if (request.NewStatus is null)
            {
                throw new ArgumentNullException(nameof(request), "Given status was null!");
            }

            if (!Enum.TryParse<Status>(request.NewStatus, true, out Status newStatusEnum))
            {
                throw new DomainException($"Unknown status {request.NewStatus}");
            }

            var existingTask = await _repo.GetByIdAsync(request.TaskId, cancellationToken);

            var oldStatus = existingTask.Status.ToString();

            //Changing status using Task entity method
            existingTask.ChangeStatus(newStatusEnum);

            await _repo.UpdateAsync(existingTask, cancellationToken);

            await _cache.RemoveAsync($"task:{request.TaskId}", cancellationToken);

            var changes = new List<string>
                {
                    $"Status changed from {oldStatus} to {request.NewStatus}"
                };

            var message = new TaskDataChanged(request.TaskId, changes, DateTime.UtcNow);

            var secondMessage = new TaskStatusChanged
            {
                TaskId = request.TaskId,
                TaskName = existingTask.Name,
                OldStatus = oldStatus,
                NewStatus = request.NewStatus
            };

            _logger.LogInformation("Successfully changed status from {oldStatus} to {newStatus} for task: {taskName}",
                oldStatus,
                request.NewStatus,
                existingTask.Name);

            await _kafkaProducer.PublishAsync(message);

            await _bus.Publish(secondMessage, cancellationToken);

        }
    }
}
