using EmployeesTasksTracker.EmployeesService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.EmployeesService.Application.Queries
{
    public record GetEmployeeByIdQuery(Guid Id) : IRequest<EmployeeDTO>;
}
