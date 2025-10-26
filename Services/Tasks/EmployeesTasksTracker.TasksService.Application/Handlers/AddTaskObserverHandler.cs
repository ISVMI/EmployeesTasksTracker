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
                await _repo.AddObserverAsync(request.ObserverId, request.TaskId, cancellationToken);

                var changes = new List<string>
                {
                    $"Добавился наблюдатель {request.ObserverId}"
                };

                var message = new TaskDataChanged(request.TaskId, changes, DateTime.UtcNow);

                await _bus.Publish(message, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not add observer: {ex.Message}");
            }
        }
    }
}
