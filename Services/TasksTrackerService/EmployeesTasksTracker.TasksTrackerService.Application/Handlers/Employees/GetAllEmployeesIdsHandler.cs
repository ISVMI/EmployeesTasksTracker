using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Employees;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Employees
{
    public class GetAllEmployeesIdsHandler : IRequestHandler<GetAllEmployeesIdsQuery, IEnumerable<Guid>>
    {

        private readonly IEmployeesRepo _repo;

        public GetAllEmployeesIdsHandler(IEmployeesRepo repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<Guid>> Handle(GetAllEmployeesIdsQuery request, CancellationToken cancellationToken)
        {
            var employeesIds = await _repo.GetAllIds(cancellationToken);

            return employeesIds;
        }
    }
}
