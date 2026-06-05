using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Messages;
using Shared.Models;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks
{
    public class AddTaskPerformerHandler : IRequestHandler<AddTaskPerformerCommand, Result>
    {
        private readonly ITasksRepo _repo;
        private readonly IBus _bus;
        private readonly ITaskEmployeeRepo _taskEmployeeRepo;
        private readonly IDistributedCache _cache;

        public AddTaskPerformerHandler(ITasksRepo repo, ITaskEmployeeRepo taskEmployeeRepo, IBus bus, IDistributedCache cache)
        {
            _repo = repo;
            _bus = bus;
            _taskEmployeeRepo = taskEmployeeRepo;
            _cache = cache;
        }

        public async Task<Result> Handle(AddTaskPerformerCommand request, CancellationToken cancellationToken)
        {
            var task = await _repo.GetByIdAsync(request.TaskId, cancellationToken);

            var employeeTask = await _taskEmployeeRepo.GetAllById(request.TaskId, request.PerformerId, cancellationToken);

            if (employeeTask.Any()) 
            {
                var employeeRole = employeeTask.First().EmployeeRoleInTask;

                return Result.Failure($"Employee with id: {request.PerformerId} already assigned as {employeeRole}!");
            }

            await _taskEmployeeRepo.AddEmployeeAsync(request.PerformerId, request.TaskId, RoleInTask.Observer, cancellationToken);

            var changes = new List<string>
                {
                    $"Добавился исполнитель {request.PerformerId}"
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

            return Result.Success();
        }
    }
}
