using MediatR;
using Shared.DTOs;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Queries.Tasks
{
    public record GetTasksByGroupIdQuery(Guid TasksGroupId) : IRequest<IEnumerable<TaskForReportDTO>>;
}
