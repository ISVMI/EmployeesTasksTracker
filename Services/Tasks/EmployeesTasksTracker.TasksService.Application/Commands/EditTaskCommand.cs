using EmployeesTasksTracker.TasksService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.TasksService.Application.Commands
{
    public record EditTaskCommand(EditTaskDTO TaskToEdit) : IRequest<TaskDTO>;
}
