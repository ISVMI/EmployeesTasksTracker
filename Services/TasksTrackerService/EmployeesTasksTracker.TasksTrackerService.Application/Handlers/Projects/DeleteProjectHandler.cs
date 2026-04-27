using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Projects;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Projects
{
    public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, bool>
    {
        private readonly IProjectsRepo _projectsRepo;

        public DeleteProjectHandler(IProjectsRepo projectsRepo)
        {
            _projectsRepo = projectsRepo;
        }

        public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var canDelete = await _projectsRepo.CheckDeletionCapability(request.Id, cancellationToken);

            if (!canDelete)
            {
                throw new DomainException($"Couldn't delete project! Task isn't completed or cancelled!");
            }

            return await _projectsRepo.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
