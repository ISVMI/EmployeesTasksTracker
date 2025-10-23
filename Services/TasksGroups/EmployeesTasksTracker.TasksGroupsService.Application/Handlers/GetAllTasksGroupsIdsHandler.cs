using EmployeesTasksTracker.TasksGroupsService.Application.Queries;
using EmployeesTasksTracker.TasksGroupsService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Handlers
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
            var tasksGroupsIds = await _repo.GetAllIdsAsync(cancellationToken);

            return tasksGroupsIds;
        }
    }
}
