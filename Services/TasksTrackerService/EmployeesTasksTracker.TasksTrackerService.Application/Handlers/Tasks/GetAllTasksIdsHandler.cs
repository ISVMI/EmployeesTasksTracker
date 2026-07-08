using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks
{
    internal class GetAllTasksIdsHandler : IRequestHandler<GetAllTasksIdsQuery, IEnumerable<Guid>>
    {
        private readonly ITasksRepo _repo;

        public GetAllTasksIdsHandler(ITasksRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Guid>> Handle(GetAllTasksIdsQuery request, CancellationToken cancellationToken)
        {
            var tasksIds = await _repo.GetAllIds(cancellationToken);

            return tasksIds;
        }
    }
}
