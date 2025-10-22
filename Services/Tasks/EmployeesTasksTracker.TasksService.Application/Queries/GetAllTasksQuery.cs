using EmployeesTasksTracker.TasksService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.TasksService.Application.Queries
{
    public record GetAllTasksQuery(Guid? EmployeeId = null, Guid? TasksGroupId = null, Guid? ProjectId = null) : IRequest<IEnumerable<TaskDTO>>;
    
}
