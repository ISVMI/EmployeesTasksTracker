using AutoMapper;
using EmployeesTasksTracker.TasksGroupsService.Application.Commands;
using EmployeesTasksTracker.TasksGroupsService.Application.DTOs;
using EmployeesTasksTracker.TasksGroupsService.Core.Interfaces;
using EmployeesTasksTracker.TasksGroupsService.Core.Models;
using MediatR;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Handlers
{
    public class EditTasksGroupHandler : IRequestHandler<EditTasksGroupCommand, TasksGroupDTO>
    {
        private readonly ITasksGroupsRepo _repo;
        private readonly IMapper _mapper;

        public EditTasksGroupHandler(ITasksGroupsRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<TasksGroupDTO> Handle(EditTasksGroupCommand request, CancellationToken cancellationToken)
        {
            var tasksGroupToEdit = _mapper.Map<TasksGroup>(request.TasksGroupToEdit);
            await _repo.UpdateAsync(tasksGroupToEdit,cancellationToken);
            return _mapper.Map<TasksGroupDTO>(tasksGroupToEdit);
        }
    }
}
