using AutoMapper;
using EmployeesTasksTracker.TasksService.Application.DTOs;
using EmployeesTasksTracker.TasksService.Application.Queries;
using EmployeesTasksTracker.TasksService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksService.Application.Handlers
{
    public class GetAllTasksHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskDTO>>
    {
        private readonly ITasksRepo _repo;
        private readonly IMapper _mapper;

        public GetAllTasksHandler(ITasksRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskDTO>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _repo.GetAllAsync(request.EmployeeId, request.TasksGroupId, request.ProjectId, cancellationToken);
            
            return _mapper.Map<IEnumerable<TaskDTO>>(tasks);
        }
    }
}
