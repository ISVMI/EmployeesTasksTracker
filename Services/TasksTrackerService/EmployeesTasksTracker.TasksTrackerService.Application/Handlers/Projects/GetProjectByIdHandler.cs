using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Projects;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Projects;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Projects
{
    public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ProjectDTO>
    {
        private readonly IProjectsRepo _repo;
        private readonly HybridCache _cache;

        public GetProjectByIdHandler(IProjectsRepo repo, HybridCache cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public async Task<ProjectDTO> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {

            var cacheKey = $"project:{request.Id}";

            var project = await _cache.GetOrCreateAsync(
                cacheKey,
                async token => await _repo.GetByIdAsync(request.Id, cancellationToken),
                cancellationToken: cancellationToken);

            return project is null
                ? throw new NotFoundException("project", request.Id)
                : new ProjectDTO
                {
                    Name = project.Name,
                    Description = project.Description
                };
        }
    }
}
