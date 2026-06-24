using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using Shared.Messages;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks
{
    public class AddTaskObserverHandler : IRequestHandler<AddTaskObserverCommand>
    {
        private readonly ITasksRepo _repo;
        private readonly IBus _bus;
        private readonly ITaskEmployeeRepo _taskEmployeeRepo;
        private readonly HybridCache _cache;
        private readonly ILogger<AddTaskObserverHandler> _logger;

        public AddTaskObserverHandler(
            ITasksRepo repo,
            ITaskEmployeeRepo taskEmployeeRepo,
            IBus bus,
            HybridCache cache,
            ILogger<AddTaskObserverHandler> logger)
        {
            _repo = repo;
            _bus = bus;
            _taskEmployeeRepo = taskEmployeeRepo;
            _cache = cache;
            _logger = logger;
        }

        public async Task Handle(AddTaskObserverCommand request, CancellationToken cancellationToken)
        {
            var task = await _repo.GetByIdAsync(request.TaskId, cancellationToken) ?? throw new NotFoundException("task", request.TaskId);

            var employeeProject = await _taskEmployeeRepo.GetAllById(request.TaskId, request.ObserverId, cancellationToken);

            if (employeeProject.Any())
            {
                var employeeRole = employeeProject.First().EmployeeRoleInTask;

                throw new DomainException($"Employee with id: {request.ObserverId} already assigned as {employeeRole}!");
            }

            await _taskEmployeeRepo.AddEmployeeAsync(request.ObserverId, request.TaskId, RoleInTask.Observer, cancellationToken);

            var changes = new List<string>
                {
                    $"Added observer with id: {request.ObserverId}"
                };

            await _cache.RemoveAsync($"task:{request.TaskId}", cancellationToken);

            var message = new TaskDataChanged(request.TaskId, changes, DateTime.UtcNow);

            var secondMessage = new EmployeeAssigned
            {
                TaskId = request.TaskId,
                EmployeeId = request.ObserverId,
                TaskName = task.Name
            };

            await _bus.Publish(message, cancellationToken);

            await _bus.Publish(secondMessage, cancellationToken);

            _logger.LogInformation("Successfully added observer with id {observerId} for task {taskId}", request.ObserverId, request.TaskId);
        }
    }
}
