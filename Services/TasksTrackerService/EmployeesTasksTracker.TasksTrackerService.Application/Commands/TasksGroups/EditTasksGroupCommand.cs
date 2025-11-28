using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.TasksGroups;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Commands.TasksGroups
{
    public record EditTasksGroupCommand(EditTasksGroupDTO TasksGroupToEdit) : IRequest<TasksGroupDTO>;
}
