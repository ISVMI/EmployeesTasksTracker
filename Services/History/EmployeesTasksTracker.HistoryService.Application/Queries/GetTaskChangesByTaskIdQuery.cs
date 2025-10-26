using EmployeesTasksTracker.HistoryService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.HistoryService.Application.Queries
{
    public record GetTaskChangesByTaskIdQuery(Guid TaskId) : IRequest<IEnumerable<TaskChangesDTO>>;
}
