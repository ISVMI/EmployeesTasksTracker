using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Messages;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks
{
    public class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly ITasksRepo _repo;
        private readonly IBus _bus;
        private readonly IDistributedCache _cache;

        public DeleteTaskHandler(ITasksRepo repo, IBus bus, IDistributedCache cache)
        {
            _repo = repo;
            _bus = bus;
            _cache = cache;
        }
        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {

            var message = new TaskDeleted(request.Id, DateTime.UtcNow);

            await _bus.Publish(message, cancellationToken);

            await _cache.RemoveAsync($"task:{request.Id}", cancellationToken);

            return await _repo.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
