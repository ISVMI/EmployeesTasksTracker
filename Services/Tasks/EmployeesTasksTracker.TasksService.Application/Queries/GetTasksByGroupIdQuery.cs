using MediatR;
using Shared.DTOs;

namespace EmployeesTasksTracker.TasksService.Application.Queries
{
    public record GetTasksByGroupIdQuery(Guid TasksGroupId) : IRequest<IEnumerable<TaskForReportDTO>>;
}
