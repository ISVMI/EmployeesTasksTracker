using EmployeesTasksTracker.ProjectsService.Application.Queries;
using EmployeesTasksTracker.ProjectsService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.ProjectsService.Application.Handlers
{
    public class GetAllProjectsIdsHandler : IRequestHandler<GetAllProjectsIdsQuery, IEnumerable<Guid>>
    {
        private readonly IProjectsRepo _repo;

        public GetAllProjectsIdsHandler(IProjectsRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Guid>> Handle(GetAllProjectsIdsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _repo.GetAllIdsAsync(cancellationToken);

            return projects;
        }
    }
}
