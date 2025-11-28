using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Projects;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Projects;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Projects
{
    public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ProjectDTO>
    {
        private readonly IProjectsRepo _repo;

        public GetProjectByIdHandler(IProjectsRepo repo)
        {
            _repo = repo;
        }

        public async Task<ProjectDTO> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
                var project = await _repo.GetByIdAsync(request.Id, cancellationToken);

                return new ProjectDTO 
                {
                    Name = project.Name,
                    Description = project.Description
                };
        }
    }
}
