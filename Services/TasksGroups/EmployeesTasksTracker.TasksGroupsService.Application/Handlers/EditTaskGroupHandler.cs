using AutoMapper;
using EmployeesTasksTracker.TasksGroupsService.Application.Commands;
using EmployeesTasksTracker.TasksGroupsService.Application.DTOs;
using EmployeesTasksTracker.TasksGroupsService.Core.Interfaces;
using EmployeesTasksTracker.TasksGroupsService.Core.Models;
using MediatR;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Handlers
{
    public class EditTaskGroupHandler : IRequestHandler<EditTaskGroupCommand, TaskGroupDto>
    {
        private readonly ITaskGroupsRepo _repo;
        private readonly IMapper _mapper;

        public EditTaskGroupHandler(ITaskGroupsRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<TaskGroupDto> Handle(EditTaskGroupCommand request, CancellationToken cancellationToken)
        {
            var taskGroupToEdit = _mapper.Map<TaskGroup>(request.TaskGRoupToEdit);
            await _repo.UpdateAsync(taskGroupToEdit,cancellationToken);
            return _mapper.Map<TaskGroupDto>(taskGroupToEdit);
        }
    }
}
