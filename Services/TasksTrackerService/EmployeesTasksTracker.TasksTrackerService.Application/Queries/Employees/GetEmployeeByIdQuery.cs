using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Employees;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Queries.Employees
{
    public record GetEmployeeByIdQuery(Guid Id) : IRequest<EmployeeDTO>;
}
