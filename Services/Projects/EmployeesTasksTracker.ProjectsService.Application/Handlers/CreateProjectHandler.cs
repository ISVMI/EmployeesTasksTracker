using AutoMapper;
using EmployeesTasksTracker.ProjectsService.Application.Commands;
using EmployeesTasksTracker.ProjectsService.Core.Interfaces;
using EmployeesTasksTracker.ProjectsService.Core.Models;
using MediatR;

namespace EmployeesTasksTracker.ProjectsService.Application.Handlers
{
    public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, Guid>
    {
        private readonly IProjectsRepo _repo;
        private readonly IMapper _mapper;

        public CreateProjectHandler(IProjectsRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newProject = _mapper.Map<Project>(request.Project);
                await _repo.CreateAsync(newProject, cancellationToken);
                return newProject.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
