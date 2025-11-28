using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Projects;
using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Projects;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Projects
{
    public class EditProjectHandler : IRequestHandler<EditProjectCommand, ProjectDTO>
    {
        private readonly IProjectsRepo _repo;

        public EditProjectHandler(IProjectsRepo repo)
        {
            _repo = repo;
        }

        public async Task<ProjectDTO> Handle(EditProjectCommand request, CancellationToken cancellationToken)
        {
            var projectToEdit = new Project
            {
                Id = request.ProjectToEdit.Id,
                Name = request.ProjectToEdit.Name,
                Description = request.ProjectToEdit.Description,
            };

            await _repo.UpdateAsync(projectToEdit, cancellationToken);

            return new ProjectDTO
            {
               Name = projectToEdit.Name,
               Description = projectToEdit.Description
            };
        }
    }
}
