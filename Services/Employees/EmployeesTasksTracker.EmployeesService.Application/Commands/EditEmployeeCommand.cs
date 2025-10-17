
using EmployeesTasksTracker.EmployeesService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.EmployeesService.Application.Commands
{
    public record EditEmployeeCommand(EditEmployeeDTO EmployeeToEdit) : IRequest<EmployeeDTO>;
}
