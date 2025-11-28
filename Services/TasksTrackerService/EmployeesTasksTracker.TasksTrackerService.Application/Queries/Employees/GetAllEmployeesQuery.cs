using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Employees;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Queries.Employees
{
    public record GetAllEmployeesQuery : IRequest<IEnumerable<EmployeeDTO>>;
}
