using AutoMapper;
using EmployeesTasksTracker.TasksGroupsService.Application.DTOs;
using EmployeesTasksTracker.TasksGroupsService.Application.Queries;
using EmployeesTasksTracker.TasksGroupsService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Handlers
{
    public class GetTasksGroupByIdHandler : IRequestHandler<GetTasksGroupByIdQuery, TasksGroupDTO>
    {
        private readonly ITasksGroupsRepo _repo;
        private readonly IMapper _mapper;

        public GetTasksGroupByIdHandler(ITasksGroupsRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<TasksGroupDTO> Handle(GetTasksGroupByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var taskGroup = await _repo.GetByIdAsync(request.Id, cancellationToken);

                return _mapper.Map<TasksGroupDTO>(taskGroup);
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get task group: {ex.Message}");
            }
        }
    }
}
