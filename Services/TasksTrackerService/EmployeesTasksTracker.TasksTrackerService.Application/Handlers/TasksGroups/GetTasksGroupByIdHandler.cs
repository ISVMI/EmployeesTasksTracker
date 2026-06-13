using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.TasksGroups
{
    public class GetTasksGroupByIdHandler : IRequestHandler<GetTasksGroupByIdQuery, TasksGroupDTO>
    {
        private readonly ITasksGroupsRepo _repo;
        private readonly HybridCache _cache;
        private readonly Logger<GetTasksGroupByIdHandler> _logger;

        public GetTasksGroupByIdHandler(ITasksGroupsRepo repo, HybridCache cache, Logger<GetTasksGroupByIdHandler> logger)
        {
            _repo = repo;
            _cache = cache;
            _logger = logger;
        }

        public async Task<TasksGroupDTO> Handle(GetTasksGroupByIdQuery request, CancellationToken cancellationToken)
        {

            var cacheKey = $"tasksgroup:{request.Id}";

            var tasksGroup = await _cache.GetOrCreateAsync(
                cacheKey,
                async token => await _repo.GetByIdAsync(request.Id, cancellationToken),
                cancellationToken: cancellationToken) ?? throw new NotFoundException("tasksGroup", request.Id);

            _logger.LogInformation("Successfully found tasks group {tasksGroupName}", tasksGroup.Name);

            return new TasksGroupDTO
            {
                Name = tasksGroup.Name
            };
        }
    }
}
