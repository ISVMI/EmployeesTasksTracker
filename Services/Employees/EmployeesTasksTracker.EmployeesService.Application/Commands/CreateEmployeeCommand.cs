using EmployeesTasksTracker.EmployeesService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.EmployeesService.Application.Commands
{
    public record CreateEmployeeCommand(EmployeeDTO Employee) : IRequest<Guid>;
}
