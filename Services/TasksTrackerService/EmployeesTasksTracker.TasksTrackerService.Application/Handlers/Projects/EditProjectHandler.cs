using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Projects;
using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Projects;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Projects
{
    public class EditProjectHandler : IRequestHandler<EditProjectCommand, ProjectDTO>
    {
        private readonly IProjectsRepo _repo;
        private readonly IDistributedCache _cache;

        public EditProjectHandler(IProjectsRepo repo, IDistributedCache cache)
        {
            _repo = repo;
            _cache = cache;
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

            await _cache.RemoveAsync($"project:{request.ProjectToEdit.Id}", cancellationToken);

            return new ProjectDTO
            {
               Name = projectToEdit.Name,
               Description = projectToEdit.Description
            };
        }
    }
}
