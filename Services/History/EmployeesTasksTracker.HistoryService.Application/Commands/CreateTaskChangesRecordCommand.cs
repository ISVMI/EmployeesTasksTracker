using EmployeesTasksTracker.HistoryService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.HistoryService.Application.Commands
{
    public record CreateTaskChangesRecordCommand(TaskChangesDTO TaskChanges) : IRequest<Guid>;
}
