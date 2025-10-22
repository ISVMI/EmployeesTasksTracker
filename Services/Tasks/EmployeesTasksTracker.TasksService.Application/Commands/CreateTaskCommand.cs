using EmployeesTasksTracker.TasksService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.TasksService.Application.Commands
{
    public record CreateTaskCommand(CreateTaskDTO Task) : IRequest<Guid>;
}
