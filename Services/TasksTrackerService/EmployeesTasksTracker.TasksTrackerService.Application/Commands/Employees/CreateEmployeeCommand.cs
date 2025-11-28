using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Employees;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Commands.Employees
{
    public record CreateEmployeeCommand(EmployeeDTO Employee) : IRequest<Guid>;
}
