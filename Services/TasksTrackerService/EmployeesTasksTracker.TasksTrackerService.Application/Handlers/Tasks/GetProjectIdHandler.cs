using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks
{
    public class GetProjectIdHandler : IRequestHandler<GetProjectIdQuery, Guid>
    {
        private readonly ITasksRepo _repo;

        public GetProjectIdHandler(ITasksRepo repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(GetProjectIdQuery request, CancellationToken cancellationToken)
        {
                var projectId = await _repo.GetProjectId(request.TasksGroupId, cancellationToken);

                return projectId;
        }
    }
}
