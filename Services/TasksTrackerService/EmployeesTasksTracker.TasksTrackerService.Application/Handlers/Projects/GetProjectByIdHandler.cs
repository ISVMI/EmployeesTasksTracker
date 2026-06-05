using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Projects;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Projects;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Extensions;
using System.Text.Json;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Projects
{
    public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ProjectDTO>
    {
        private readonly IProjectsRepo _repo;
        private readonly IDistributedCache _cache;

        public GetProjectByIdHandler(IProjectsRepo repo, IDistributedCache cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public async Task<ProjectDTO> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {

            var cacheKey = $"project:{request.Id}";

            var cachedProject = await _cache.GetRecordAsync<Project>(cacheKey);

            var project = cachedProject ?? await _repo.GetByIdAsync(request.Id, cancellationToken);

            if (cachedProject == null)
            {
                var serializedProject = JsonSerializer.Serialize(project);

                var expirationTime = TimeSpan.FromMinutes(30);

                await _cache.SetRecordAsync(cacheKey, serializedProject, expirationTime);
            }

            return new ProjectDTO
            {
                Name = project.Name,
                Description = project.Description
            };
        }
    }
}
