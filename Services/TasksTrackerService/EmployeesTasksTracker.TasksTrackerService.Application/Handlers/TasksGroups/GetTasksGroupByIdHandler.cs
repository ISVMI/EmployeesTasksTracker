using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.TasksGroups
{
    public class GetTasksGroupByIdHandler : IRequestHandler<GetTasksGroupByIdQuery, TasksGroupDTO>
    {
        private readonly ITasksGroupsRepo _repo;
        private readonly HybridCache _cache;

        public GetTasksGroupByIdHandler(ITasksGroupsRepo repo, HybridCache cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public async Task<TasksGroupDTO> Handle(GetTasksGroupByIdQuery request, CancellationToken cancellationToken)
        {

            var cacheKey = $"tasksgroup:{request.Id}";

            var cachedTasksGroup = await _cache.GetOrCreateAsync(
                cacheKey,
                async token => await _repo.GetByIdAsync(request.Id, cancellationToken),
                cancellationToken: cancellationToken);

            var tasksGroup = cachedTasksGroup ?? await _repo.GetByIdAsync(request.Id, cancellationToken);

            return new TasksGroupDTO
            {
                Name = tasksGroup.Name
            };
        }
    }
}
