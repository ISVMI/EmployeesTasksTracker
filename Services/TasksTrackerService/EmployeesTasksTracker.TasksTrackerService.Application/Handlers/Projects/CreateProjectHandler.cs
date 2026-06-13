using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Projects;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Projects
{
    public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, Guid>
    {
        private readonly IProjectsRepo _repo;
        private readonly ILogger<CreateProjectHandler> _logger;

        public CreateProjectHandler(IProjectsRepo repo, ILogger<CreateProjectHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var newProject = new Project
            {
                Name = request.Project.Name,
                Description = request.Project.Description
            };

            var result = await _repo.CreateAsync(newProject, cancellationToken);

            _logger.LogInformation("New project created with id {projectId}", result);

            return result;
        }
    }
}
