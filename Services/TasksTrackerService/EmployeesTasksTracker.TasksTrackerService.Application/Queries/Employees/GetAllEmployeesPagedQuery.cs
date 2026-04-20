using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Employees;
using MediatR;
using Shared.DTOs;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Queries.Employees
{
    public record GetAllEmployeesPagedQuery(int Page = 1, int PageSize = 20) : IRequest<PagedResponse<EmployeeDTO>>;
}
