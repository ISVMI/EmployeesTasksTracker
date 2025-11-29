using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MassTransit;
using MediatR;
using Shared.Messages;
using Shared.Models;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks
{
    public class AddTaskPerformerHandler : IRequestHandler<AddTaskPerformerCommand, Result>
    {
        private readonly ITasksRepo _repo;
        private readonly IBus _bus;
        private readonly ITaskEmployeeRepo _taskEmployeeRepo;

        public AddTaskPerformerHandler(ITasksRepo repo, ITaskEmployeeRepo taskEmployeeRepo, IBus bus)
        {
            _repo = repo;
            _bus = bus;
            _taskEmployeeRepo = taskEmployeeRepo;
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

            await _repo.AddPerformerAsync(request.PerformerId, request.TaskId, cancellationToken);

            var changes = new List<string>
                {
                    $"Добавился исполнитель {request.PerformerId}"
                };

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
