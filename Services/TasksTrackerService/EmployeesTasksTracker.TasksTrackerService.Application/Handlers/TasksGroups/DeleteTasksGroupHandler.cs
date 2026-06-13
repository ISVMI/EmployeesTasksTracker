using EmployeesTasksTracker.TasksTrackerService.Application.Commands.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.TasksGroups
{
    public class DeleteTasksGroupHandler : IRequestHandler<DeleteTasksGroupCommand, bool>
    {
        private readonly ITasksGroupsRepo _tasksGroupsRepo;
        private readonly HybridCache _cache;
        private readonly ILogger<DeleteTasksGroupHandler> _logger;

        public DeleteTasksGroupHandler(ITasksGroupsRepo tasksGroupsRepo, HybridCache cache, ILogger<DeleteTasksGroupHandler> logger)
        {
            _tasksGroupsRepo = tasksGroupsRepo;
            _cache = cache;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteTasksGroupCommand request, CancellationToken cancellationToken)
        {
            var canDelete = await _tasksGroupsRepo.CheckDeletionCapability(request.Id, cancellationToken);

            await _cache.RemoveAsync($"tasksgroup:{request.Id}", cancellationToken);

            if (!canDelete)
            {
                throw new DomainException($"Couldn't delete tasks group! Task isn't completed or cancelled!");
            }

            _logger.LogInformation("Successfully deleted tasks group with id {tasksGroupId}", request.Id);

            return await _tasksGroupsRepo.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
