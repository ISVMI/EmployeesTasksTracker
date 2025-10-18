using AutoMapper;
using EmployeesTasksTracker.ProjectsService.Application.Commands;
using EmployeesTasksTracker.ProjectsService.Application.DTOs;
using EmployeesTasksTracker.ProjectsService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.ProjectsService.Application.Handlers
{
    public class EditProjectHandler : IRequestHandler<EditProjectCommand, ProjectDTO>
    {
        private readonly IProjectsRepo _repo;
        private readonly IMapper _mapper;

        public EditProjectHandler(IProjectsRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ProjectDTO> Handle(EditProjectCommand request, CancellationToken cancellationToken)
        {
            var projectToEdit = await _repo.GetByIdAsync(request.ProjectToEdit.Id, cancellationToken);
            await _repo.UpdateAsync(projectToEdit, cancellationToken);
            return _mapper.Map<ProjectDTO>(projectToEdit);
        }
    }
}
