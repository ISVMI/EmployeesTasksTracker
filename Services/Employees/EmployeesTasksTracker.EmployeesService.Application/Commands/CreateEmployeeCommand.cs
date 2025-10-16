using EmployeesTasksTracker.EmployeesService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.EmployeesService.Application.Commands
{
    public record CreateEmployeeCommand(CreateEmployeeDTO Employee) : IRequest<Guid>;
}
