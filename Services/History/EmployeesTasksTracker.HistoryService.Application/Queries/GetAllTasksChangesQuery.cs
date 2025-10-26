using EmployeesTasksTracker.HistoryService.Application.DTOs;
using MediatR;

namespace EmployeesTasksTracker.HistoryService.Application.Queries
{
    public record GetAllTasksChangesQuery : IRequest<IEnumerable<TaskChangesDTO>>;
}
