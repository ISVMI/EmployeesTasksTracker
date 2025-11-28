using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Tasks;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks
{
    public record EditTaskCommand(EditTaskDTO TaskToEdit) : IRequest<TaskDTO>;
}
