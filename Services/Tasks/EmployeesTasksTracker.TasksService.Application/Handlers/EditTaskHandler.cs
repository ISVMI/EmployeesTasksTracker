using AutoMapper;
using EmployeesTasksTracker.TasksService.Application.Commands;
using EmployeesTasksTracker.TasksService.Application.DTOs;
using EmployeesTasksTracker.TasksService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksService.Application.Handlers
{
    public class EditTaskHandler : IRequestHandler<EditTaskCommand, TaskDTO>
    {
        private readonly ITasksRepo _repo;
        private readonly IMapper _mapper;

        public EditTaskHandler(ITasksRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<TaskDTO> Handle(EditTaskCommand request, CancellationToken cancellationToken)
        {
            var taskToEdit = _mapper.Map<Core.Models.Task>(request.TaskToEdit);
            await _repo.UpdateAsync(taskToEdit, cancellationToken);
            return _mapper.Map<TaskDTO>(taskToEdit);
        }
    }
}
