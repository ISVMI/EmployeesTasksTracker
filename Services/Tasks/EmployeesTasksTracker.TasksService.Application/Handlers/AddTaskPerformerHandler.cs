using EmployeesTasksTracker.TasksService.Application.Commands;
using EmployeesTasksTracker.TasksService.Core.Interfaces;
using MassTransit;
using MediatR;
using Shared.Messages;

namespace EmployeesTasksTracker.TasksService.Application.Handlers
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
            try
            {
                await _repo.AddPerformerAsync(request.PerformerId, request.TaskId, cancellationToken);

                var changes = new List<string>
                {
                    $"Добавился исполнитель {request.PerformerId}"
                };

                var message = new TaskDataChanged(request.TaskId, changes, DateTime.UtcNow);

                await _bus.Publish(message, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not add performer: {ex.Message}");
            }
        }
    }
}
