using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Projects;
using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Projects;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Projects
{
    public class EditProjectHandler : IRequestHandler<EditProjectCommand, ProjectDTO>
    {
        private readonly IProjectsRepo _repo;
        private readonly HybridCache _cache;

        public EditProjectHandler(IProjectsRepo repo, HybridCache cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public async Task<ProjectDTO> Handle(EditProjectCommand request, CancellationToken cancellationToken)
        {

            var projectToEdit = await _repo.GetByIdAsync(request.ProjectToEdit.Id, cancellationToken)
                ?? throw new NotFoundException("project", request.ProjectToEdit.Id);

            projectToEdit.Id = request.ProjectToEdit.Id;
            projectToEdit.Name = request.ProjectToEdit.Name;
            projectToEdit.Description = request.ProjectToEdit.Description;


            await _cache.RemoveAsync($"project:{request.ProjectToEdit.Id}", cancellationToken);

            var editedProject = await _repo.UpdateAsync(projectToEdit, cancellationToken);

            return editedProject is null
                ? throw new NotFoundException("project", request.ProjectToEdit.Id)
                : new ProjectDTO
                {
                    Name = projectToEdit.Name,
                    Description = projectToEdit.Description
                };
        }
    }
}
