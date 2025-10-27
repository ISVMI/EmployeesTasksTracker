using EmployeesTasksTracker.TasksService.Application.Commands;
using EmployeesTasksTracker.TasksService.Core.Interfaces;
using MassTransit;
using MediatR;
using Shared.Messages;

namespace EmployeesTasksTracker.TasksService.Application.Handlers
{
    public class AddTaskObserverHandler : IRequestHandler<AddTaskObserverCommand>
    {
        private readonly ITasksRepo _repo;
        private readonly IBus _bus;

        public AddTaskObserverHandler(ITasksRepo repo, IBus bus)
        {
            _repo = repo;
            _bus = bus;
        }

        public async Task Handle(AddTaskObserverCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _repo.GetByIdAsync(request.TaskId, cancellationToken);

                await _repo.AddObserverAsync(request.ObserverId, request.TaskId, cancellationToken);

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
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not add observer: {ex.Message}");
            }
        }
    }
}
