using AutoMapper;
using EmployeesTasksTracker.ProjectsService.Application.DTOs;
using EmployeesTasksTracker.ProjectsService.Application.Queries;
using EmployeesTasksTracker.ProjectsService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.ProjectsService.Application.Handlers
{
    public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ProjectDTO>
    {
        private readonly IProjectsRepo _repo;
        private readonly IMapper _mapper;

        public GetProjectByIdHandler(IProjectsRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ProjectDTO> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var project = await _repo.GetByIdAsync(request.Id, cancellationToken);

                return _mapper.Map<ProjectDTO>(project);
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get project: {ex.Message}");
            }
        }
    }
}
