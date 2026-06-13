using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Projects;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Projects
{
    public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, bool>
    {
        private readonly IProjectsRepo _projectsRepo;
        private readonly HybridCache _cache;
        private readonly ILogger<DeleteProjectHandler> _logger;

        public DeleteProjectHandler(IProjectsRepo projectsRepo, HybridCache cache, ILogger<DeleteProjectHandler> logger)
        {
            _projectsRepo = projectsRepo;
            _cache = cache;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var canDelete = await _projectsRepo.CheckDeletionCapability(request.Id, cancellationToken);

            if (!canDelete)
            {
                throw new DomainException($"Couldn't delete project! Task isn't completed or cancelled!");
            }

            await _cache.RemoveAsync($"project:{request.Id}", cancellationToken);

            var result = await _projectsRepo.DeleteAsync(request.Id, cancellationToken);

            _logger.LogInformation("Successfully deleted project with id {projectId}", request.Id);

            return result;
        }
    }
}
