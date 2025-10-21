using EmployeesTasksTracker.TasksGroupsService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Commands
{
    public record EditTasksGroupCommand(EditTasksGroupDTO TaskGRoupToEdit) : IRequest<TasksGroupDTO>;
}
