using AutoMapper;
using EmployeesTasksTracker.ProjectsService.Application.DTOs;
using EmployeesTasksTracker.ProjectsService.Application.Queries;
using EmployeesTasksTracker.ProjectsService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.ProjectsService.Application.Handlers
{
    public class GetAllProjectsHandler : IRequestHandler<GetAllProjectsQuery, IEnumerable<ProjectDTO>>
    {
        private readonly IProjectsRepo _repo;
        private readonly IMapper _mapper;

        public GetAllProjectsHandler(IProjectsRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectDTO>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _repo.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ProjectDTO>>(projects);
        }
    }
}
