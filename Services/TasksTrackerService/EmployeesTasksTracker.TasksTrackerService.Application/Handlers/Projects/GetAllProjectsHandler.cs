using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Projects;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Projects;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Projects
{
    public class GetAllProjectsHandler : IRequestHandler<GetAllProjectsQuery, IEnumerable<ProjectDTO>>
    {
        private readonly IProjectsRepo _repo;

        public GetAllProjectsHandler(IProjectsRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ProjectDTO>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _repo.GetAllAsync(cancellationToken);

            var projectsDtoList = new List<ProjectDTO>();

            foreach (var project in projects) 
            {
                projectsDtoList.Add(new ProjectDTO
                {
                    Name = project.Name,
                    Description = project.Description
                });
            }

            return projectsDtoList;
        }
    }
}
