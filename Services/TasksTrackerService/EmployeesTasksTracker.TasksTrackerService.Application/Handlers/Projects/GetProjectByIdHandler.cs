using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Projects;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Projects;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Projects
{
    public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ProjectDTO>
    {
        private readonly IProjectsRepo _repo;
        private readonly HybridCache _cache;
        private readonly ILogger<GetProjectByIdHandler> _logger;

        public GetProjectByIdHandler(IProjectsRepo repo, HybridCache cache, ILogger<GetProjectByIdHandler> logger)
        {
            _repo = repo;
            _cache = cache;
            _logger = logger;
        }

        public async Task<ProjectDTO> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {

            var cacheKey = $"project:{request.Id}";

            var project = await _cache.GetOrCreateAsync(
                cacheKey,
                async token => await _repo.GetByIdAsync(request.Id, cancellationToken),
                cancellationToken: cancellationToken) ?? throw new NotFoundException("project", request.Id);

                _logger.LogInformation("Successfully found project {ProjectName}", project.Name);

            return new ProjectDTO
                {
                    Name = project.Name,
                    Description = project.Description
                };
        }
    }
}
