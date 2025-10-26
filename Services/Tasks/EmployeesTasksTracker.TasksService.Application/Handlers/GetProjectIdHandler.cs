using EmployeesTasksTracker.TasksService.Application.Queries;
using EmployeesTasksTracker.TasksService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksService.Application.Handlers
{
    public class GetProjectIdHandler : IRequestHandler<GetProjectIdQuery, Guid>
    {
        private readonly ITasksRepo _repo;

        public GetProjectIdHandler(ITasksRepo repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(GetProjectIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var projectId = await _repo.GetProjectId(request.TasksGroupId, cancellationToken);

                return projectId;
            }
            catch (Exception ex) 
            {
                throw new Exception($"Could not get projectId {ex.Message}");
            }
        }
    }
}
