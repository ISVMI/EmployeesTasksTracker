using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MassTransit;
using MediatR;
using Shared.Messages;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks
{
    public class AddTaskPerformerHandler : IRequestHandler<AddTaskPerformerCommand>
    {
        private readonly ITasksRepo _repo;
        private readonly IBus _bus;

        public AddTaskPerformerHandler(ITasksRepo repo, IBus bus)
        {
            _repo = repo;
            _bus = bus;
        }

        public async Task Handle(AddTaskPerformerCommand request, CancellationToken cancellationToken)
        {
                var task = await _repo.GetByIdAsync(request.TaskId, cancellationToken);

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
        }
    }
}
