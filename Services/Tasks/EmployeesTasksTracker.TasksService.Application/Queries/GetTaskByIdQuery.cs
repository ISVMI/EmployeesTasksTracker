using EmployeesTasksTracker.TasksService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.TasksService.Application.Queries
{
    public record GetTaskByIdQuery(Guid Id) : IRequest<TaskDTO>;
}
