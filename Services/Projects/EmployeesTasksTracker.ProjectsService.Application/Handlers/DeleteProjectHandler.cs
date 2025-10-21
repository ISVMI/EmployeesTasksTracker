using EmployeesTasksTracker.ProjectsService.Application.Commands;
using EmployeesTasksTracker.ProjectsService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.ProjectsService.Application.Handlers
{
    public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, bool>
    {
        private readonly IProjectsRepo _repo;

        public DeleteProjectHandler(IProjectsRepo repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            return await _repo.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
