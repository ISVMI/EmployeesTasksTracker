using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Extensions;
using System.Text.Json;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.TasksGroups
{
    public class GetTasksGroupByIdHandler : IRequestHandler<GetTasksGroupByIdQuery, TasksGroupDTO>
    {
        private readonly ITasksGroupsRepo _repo;
        private readonly IDistributedCache _cache;

        public GetTasksGroupByIdHandler(ITasksGroupsRepo repo, IDistributedCache cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public async Task<TasksGroupDTO> Handle(GetTasksGroupByIdQuery request, CancellationToken cancellationToken)
        {

            var cacheKey = $"tasksgroup:{request.Id}";

            var cachedTasksGroup = await _cache.GetRecordAsync<TasksGroup>(cacheKey);

            var tasksGroup = cachedTasksGroup ?? await _repo.GetByIdAsync(request.Id, cancellationToken);

            if (cachedTasksGroup == null)
            {
                var serializedTasksGroup = JsonSerializer.Serialize(tasksGroup);

                var expirationTime = TimeSpan.FromMinutes(30);

                await _cache.SetRecordAsync(cacheKey, serializedTasksGroup, expirationTime);
            }

            return new TasksGroupDTO
            {
                Name = tasksGroup.Name
            };
        }
    }
}
