using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.TasksGroups;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Queries.TasksGroups
{
    public record GetTasksGroupByIdQuery(Guid Id) : IRequest<TasksGroupDTO>;
}
