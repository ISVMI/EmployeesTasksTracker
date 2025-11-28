using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Tasks;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks
{
    public record CreateTaskCommand(CreateTaskDTO Task) : IRequest<Guid>;
}
