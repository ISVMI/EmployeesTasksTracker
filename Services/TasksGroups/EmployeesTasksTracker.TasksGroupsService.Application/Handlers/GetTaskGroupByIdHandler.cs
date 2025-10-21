using AutoMapper;
using EmployeesTasksTracker.TasksGroupsService.Application.DTOs;
using EmployeesTasksTracker.TasksGroupsService.Application.Queries;
using EmployeesTasksTracker.TasksGroupsService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Handlers
{
    public class GetTaskGroupByIdHandler : IRequestHandler<GetTaskGroupByIdQuery, TaskGroupDto>
    {
        private readonly ITaskGroupsRepo _repo;
        private readonly IMapper _mapper;

        public GetTaskGroupByIdHandler(ITaskGroupsRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<TaskGroupDto> Handle(GetTaskGroupByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var taskGroup = await _repo.GetByIdAsync(request.Id, cancellationToken);

                return _mapper.Map<TaskGroupDto>(taskGroup);
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get task group: {ex.Message}");
            }
        }
    }
}
