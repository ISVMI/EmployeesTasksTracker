using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Employees;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Commands.Employees
{
    public record EditEmployeeCommand(EditEmployeeDTO EmployeeToEdit) : IRequest<EmployeeDTO>;
}
