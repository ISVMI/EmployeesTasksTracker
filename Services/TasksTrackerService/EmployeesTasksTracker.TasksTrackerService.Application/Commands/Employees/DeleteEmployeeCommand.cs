using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Commands.Employees
{
    public record DeleteEmployeeCommand(Guid Id) : IRequest<bool>;
}
