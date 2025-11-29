using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MassTransit;
using MediatR;
using Shared.Messages;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks
{
    public class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly ITasksRepo _repo;
        private readonly IBus _bus;

        public DeleteTaskHandler(ITasksRepo repo, IBus bus)
        {
            _repo = repo;
            _bus = bus;
        }
        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {

            var message = new TaskDeleted(request.Id, DateTime.UtcNow);

            await _bus.Publish(message, cancellationToken);

            return await _repo.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
