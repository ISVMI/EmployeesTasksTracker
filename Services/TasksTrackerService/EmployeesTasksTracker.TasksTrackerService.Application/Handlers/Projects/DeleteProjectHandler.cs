using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Projects;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Projects
{
    public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, bool>
    {
        private readonly IProjectsRepo _projectsRepo;
        private readonly IDistributedCache _cache;

        public DeleteProjectHandler(IProjectsRepo projectsRepo, IDistributedCache cache)
        {
            _projectsRepo = projectsRepo;
            _cache = cache;
        }

        public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var canDelete = await _projectsRepo.CheckDeletionCapability(request.Id, cancellationToken);

            if (!canDelete)
            {
                throw new DomainException($"Couldn't delete project! Task isn't completed or cancelled!");
            }

            await _cache.RemoveAsync($"project:{request.Id}", cancellationToken);

            return await _projectsRepo.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
