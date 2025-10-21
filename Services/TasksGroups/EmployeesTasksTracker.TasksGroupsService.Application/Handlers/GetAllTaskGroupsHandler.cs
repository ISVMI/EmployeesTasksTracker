using AutoMapper;
using EmployeesTasksTracker.TasksGroupsService.Application.Commands;
using EmployeesTasksTracker.TasksGroupsService.Application.DTOs;
using EmployeesTasksTracker.TasksGroupsService.Application.Queries;
using EmployeesTasksTracker.TasksGroupsService.Core.Interfaces;
using EmployeesTasksTracker.TasksGroupsService.Core.Models;
using MediatR;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Handlers
{
    public class GetAllTaskGroupsHandler : IRequestHandler<GetAllTaskGroupsQuery, IEnumerable<TaskGroupDto>>
    {
        private readonly ITaskGroupsRepo _repo;
        private readonly IMapper _mapper;

        public GetAllTaskGroupsHandler(ITaskGroupsRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskGroupDto>> Handle(GetAllTaskGroupsQuery request, CancellationToken cancellationToken)
        {
            var taskGroups = await _repo.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TaskGroupDto>>(taskGroups);
        }
    }
}
