using EmployeesTasksTracker.TasksTrackerService.Application.Queries.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.TasksGroups
{
    public class GetAllTasksGroupsIdsHandler : IRequestHandler<GetAllTasksGroupsIdsQuery, IEnumerable<Guid>>
    {
        private readonly ITasksGroupsRepo _repo;

        public GetAllTasksGroupsIdsHandler(ITasksGroupsRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Guid>> Handle(GetAllTasksGroupsIdsQuery request, CancellationToken cancellationToken)
        {
            var tasksGroupsIds = await _repo.GetAllIds(cancellationToken);

            return tasksGroupsIds;
        }
    }
}
