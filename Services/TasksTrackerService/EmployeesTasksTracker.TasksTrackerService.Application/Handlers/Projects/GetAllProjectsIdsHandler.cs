using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Projects;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Projects
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
            var projects = await _repo.GetAllIds(cancellationToken);

            return projects;
        }
    }
}
