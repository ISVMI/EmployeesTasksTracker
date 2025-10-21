using MediatR;

namespace EmployeesTasksTracker.EmployeesService.Application.Commands
{
    public record DeleteEmployeeCommand(Guid Id) : IRequest<bool>;
}
