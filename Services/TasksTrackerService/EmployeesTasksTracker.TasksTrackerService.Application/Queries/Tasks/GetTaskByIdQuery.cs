using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Tasks;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Queries.Tasks
{
    public record GetTaskByIdQuery(Guid Id) : IRequest<TaskDTO>;
}
