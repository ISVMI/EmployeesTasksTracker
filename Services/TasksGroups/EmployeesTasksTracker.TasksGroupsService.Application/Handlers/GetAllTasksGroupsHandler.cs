using AutoMapper;
using EmployeesTasksTracker.TasksGroupsService.Application.DTOs;
using EmployeesTasksTracker.TasksGroupsService.Application.Queries;
using EmployeesTasksTracker.TasksGroupsService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Handlers
{
    public class GetAllTasksGroupsHandler : IRequestHandler<GetAllTasksGroupsQuery, IEnumerable<TasksGroupDTO>>
    {
        private readonly ITasksGroupsRepo _repo;
        private readonly IMapper _mapper;

        public GetAllTasksGroupsHandler(ITasksGroupsRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TasksGroupDTO>> Handle(GetAllTasksGroupsQuery request, CancellationToken cancellationToken)
        {
            var taskGroups = await _repo.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TasksGroupDTO>>(taskGroups);
        }
    }
}
