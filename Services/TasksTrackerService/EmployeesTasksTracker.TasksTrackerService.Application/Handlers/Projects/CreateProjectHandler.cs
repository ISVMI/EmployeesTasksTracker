using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Projects;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using MediatR;

namespace EmployeesTasksTracker.ProjectsService.Application.Handlers
{
    public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, Guid>
    {
        private readonly IProjectsRepo _repo;

        public CreateProjectHandler(IProjectsRepo repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
                var newProject = new Project
                {
                    Name = request.Project.Name,
                    Description = request.Project.Description
                };

                await _repo.CreateAsync(newProject, cancellationToken);

                return newProject.Id;
        }
    }
}
