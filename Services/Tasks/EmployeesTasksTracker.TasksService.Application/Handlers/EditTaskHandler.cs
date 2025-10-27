using AutoMapper;
using EmployeesTasksTracker.TasksService.Application.Commands;
using EmployeesTasksTracker.TasksService.Application.DTOs;
using EmployeesTasksTracker.TasksService.Core.Interfaces;
using MassTransit;
using MediatR;
using Shared.Messages;
using Shared.Methods;

namespace EmployeesTasksTracker.TasksService.Application.Handlers
{
    public class EditTaskHandler : IRequestHandler<EditTaskCommand, TaskDTO>
    {
        private readonly ITasksRepo _repo;
        private readonly IMapper _mapper;
        private readonly IBus _bus;

        public EditTaskHandler(ITasksRepo repo, IMapper mapper, IBus bus)
        {
            _repo = repo;
            _mapper = mapper;
            _bus = bus;
        }

        public async Task<TaskDTO> Handle(EditTaskCommand request, CancellationToken cancellationToken)
        {
            var taskToEdit = _mapper.Map<Core.Models.Task>(request.TaskToEdit);

            var existingTask = await _repo.GetByIdAsync(taskToEdit.Id, cancellationToken);

            taskToEdit.Status = existingTask.Status;

            var changes = ChangesTracker.GetChanges(existingTask, taskToEdit);

            await _repo.UpdateAsync(taskToEdit, cancellationToken);

            if (changes.Any())
            {
                var message = new TaskDataChanged(existingTask.Id, changes, DateTime.UtcNow);

                await _bus.Publish(message, cancellationToken);
            }

            return _mapper.Map<TaskDTO>(taskToEdit);
        }
    }
}
