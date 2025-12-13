using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MassTransit;
using MediatR;
using Shared.Messages;
using Shared.Models;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks
{
    public class AddTaskObserverHandler : IRequestHandler<AddTaskObserverCommand, Result>
    {
        private readonly ITasksRepo _repo;
        private readonly IBus _bus;
        private readonly ITaskEmployeeRepo _taskEmployeeRepo;

        public AddTaskObserverHandler(ITasksRepo repo, ITaskEmployeeRepo taskEmployeeRepo, IBus bus)
        {
            _repo = repo;
            _bus = bus;
            _taskEmployeeRepo = taskEmployeeRepo;
        }

        public async Task<Result> Handle(AddTaskObserverCommand request, CancellationToken cancellationToken)
        {
            var task = await _repo.GetByIdAsync(request.TaskId, cancellationToken);

            var employeeProject = await _taskEmployeeRepo.GetAllById(request.TaskId, request.ObserverId, cancellationToken);

            if (employeeProject.Any())
            {
                var employeeRole = employeeProject.First().EmployeeRoleInTask;

                return Result.Failure($"Employee with id: {request.ObserverId} already assigned as {employeeRole}!");
            }

            await _taskEmployeeRepo.AddEmployeeAsync(request.ObserverId, request.TaskId, RoleInTask.Observer, cancellationToken);

            var changes = new List<string>
                {
                    $"Добавился наблюдатель {request.ObserverId}"
                };

            var message = new TaskDataChanged(request.TaskId, changes, DateTime.UtcNow);

            var secondMessage = new EmployeeAssigned
            {
                TaskId = request.TaskId,
                EmployeeId = request.ObserverId,
                TaskName = task.Name
            };

            await _bus.Publish(message, cancellationToken);

            await _bus.Publish(secondMessage, cancellationToken);

            return Result.Success();
        }
    }
}
