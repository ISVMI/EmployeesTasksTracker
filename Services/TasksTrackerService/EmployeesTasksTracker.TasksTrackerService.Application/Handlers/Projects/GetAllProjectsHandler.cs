using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Projects;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Projects;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Projects
{
    public class GetAllProjectsHandler : IRequestHandler<GetAllProjectsQuery, IEnumerable<ProjectDTO>>
    {
        private readonly IProjectsRepo _repo;
        private readonly ILogger<GetAllProjectsHandler> _logger;

        public GetAllProjectsHandler(IProjectsRepo repo, ILogger<GetAllProjectsHandler> logger)
        {
            _repo = repo;
            _logger = logger;
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

            _logger.LogInformation("Successfully got {totalCount} projects", projects.Count());

            return projectsDtoList;
        }
    }
}
