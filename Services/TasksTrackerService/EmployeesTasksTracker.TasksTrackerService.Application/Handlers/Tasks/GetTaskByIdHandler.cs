using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Extensions;
using System.Text.Json;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks
{
    public class GetTaskByIdHandler : IRequestHandler<GetTaskByIdQuery, TaskDTO>
    {
        private readonly ITasksRepo _repo;
        private readonly IDistributedCache _cache;

        public GetTaskByIdHandler(ITasksRepo repo, IDistributedCache cache)
        {
            _repo = repo;
            _cache = cache;
        }
        public async Task<TaskDTO> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"task:{request.Id}";

            var cachedTask = await _cache.GetRecordAsync<Core.Models.Task>(cacheKey);

            var task = cachedTask ?? await _repo.GetByIdAsync(request.Id, cancellationToken);

            if (cachedTask == null)
            {
                var expirationTime = TimeSpan.FromMinutes(30);

                await _cache.SetRecordAsync(cacheKey, task, expirationTime);
            }

            return new TaskDTO
            {
                Name = task.Name,
                CreatedAt = task.CreatedAt,
                Deadline = task.Deadline,
                Description = task.Description,
                Priority = task.Priority.ToString(),
                Status = task.Status.ToString(),
            };
        }
    }
}
