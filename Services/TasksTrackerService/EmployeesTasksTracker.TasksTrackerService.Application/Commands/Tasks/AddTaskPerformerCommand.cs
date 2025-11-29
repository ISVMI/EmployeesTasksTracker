using MediatR;
using Shared.Models;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks
{
    public record AddTaskPerformerCommand(Guid PerformerId, Guid TaskId) : IRequest<Result>;
}
