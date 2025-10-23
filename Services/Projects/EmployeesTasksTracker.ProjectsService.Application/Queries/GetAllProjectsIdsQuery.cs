using MediatR;

namespace EmployeesTasksTracker.ProjectsService.Application.Queries
{
    public record GetAllProjectsIdsQuery : IRequest<IEnumerable<Guid>>;
}
