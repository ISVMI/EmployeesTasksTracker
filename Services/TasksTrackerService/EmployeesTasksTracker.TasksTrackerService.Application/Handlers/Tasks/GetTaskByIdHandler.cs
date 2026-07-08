using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks
{
    public class GetTaskByIdHandler : IRequestHandler<GetTaskByIdQuery, TaskDTO>
    {
        private readonly ITasksRepo _repo;
        private readonly HybridCache _cache;
        private readonly ILogger<GetTaskByIdHandler> _logger;

        public GetTaskByIdHandler(ITasksRepo repo, HybridCache cache, ILogger<GetTaskByIdHandler> logger)
        {
            _repo = repo;
            _cache = cache;
            _logger = logger;
        }
        public async Task<TaskDTO> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"task:{request.Id}";

            var task = await _cache.GetOrCreateAsync(
                cacheKey,
                async token => await _repo.GetByIdAsync(request.Id, cancellationToken),
                cancellationToken: cancellationToken) ?? throw new NotFoundException("task", request.Id);

            _logger.LogInformation("Successfully found task {taskName}", task.Name);

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
