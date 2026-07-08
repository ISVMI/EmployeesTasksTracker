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
    public class AddTaskPerformerHandler : IRequestHandler<AddTaskPerformerCommand>
    {
        private readonly ITasksRepo _repo;
        private readonly IBus _bus;
        private readonly ITaskEmployeeRepo _taskEmployeeRepo;
        private readonly HybridCache _cache;
        private readonly ILogger<AddTaskPerformerHandler> _logger;

        public AddTaskPerformerHandler(
            ITasksRepo repo,
            ITaskEmployeeRepo taskEmployeeRepo,
            IBus bus,
            HybridCache cache,
            ILogger<AddTaskPerformerHandler> logger)
        {
            _repo = repo;
            _bus = bus;
            _taskEmployeeRepo = taskEmployeeRepo;
            _cache = cache;
            _logger = logger;
        }

        public async Task Handle(AddTaskPerformerCommand request, CancellationToken cancellationToken)
        {
            var task = await _repo.GetByIdAsync(request.TaskId, cancellationToken);

            var taskEmployee = await _taskEmployeeRepo.GetAllById(request.TaskId, request.PerformerId, cancellationToken);

            if (taskEmployee.Any())
            {
                var employeeRole = taskEmployee.First().EmployeeRoleInTask;

                throw new DomainException($"Employee with id: {request.PerformerId} already assigned as {employeeRole}!");
            }

            await _taskEmployeeRepo.AddEmployeeAsync(request.PerformerId, request.TaskId, RoleInTask.Observer, cancellationToken);

            var changes = new List<string>
                {
                    $"Added performer with id: {request.PerformerId}"
                };

            await _cache.RemoveAsync($"task:{request.TaskId}", cancellationToken);

            var message = new TaskDataChanged(request.TaskId, changes, DateTime.UtcNow);

            var secondMessage = new EmployeeAssigned
            {
                TaskId = request.TaskId,
                EmployeeId = request.PerformerId,
                TaskName = task.Name
            };

            await _bus.Publish(message, cancellationToken);

            await _bus.Publish(secondMessage, cancellationToken);

            _logger.LogInformation("Successfully added performer with id {performerId} for task {taskId}", request.PerformerId, request.TaskId);
        }
    }
}
