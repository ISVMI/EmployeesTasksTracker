using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Tasks;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Queries.Tasks
{
    public record GetAllTasksQuery(Guid? EmployeeId = null, Guid? TasksGroupId = null, Guid? ProjectId = null) : IRequest<IEnumerable<TaskDTO>>;

}
