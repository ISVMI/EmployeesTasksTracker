using MediatR;

namespace EmployeesTasksTracker.TasksService.Application.Commands
{
    public record AddTaskPerformerCommand(Guid PerformerId, Guid TaskId) : IRequest;
}
