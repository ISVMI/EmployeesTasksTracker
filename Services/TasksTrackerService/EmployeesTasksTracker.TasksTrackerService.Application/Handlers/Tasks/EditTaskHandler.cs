using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MassTransit;
using MediatR;
using Shared.Exceptions;
using Shared.Messages;
using Shared.Methods;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks
{
    public class EditTaskHandler : IRequestHandler<EditTaskCommand, TaskDTO>
    {
        private readonly ITasksRepo _repo;
        private readonly IBus _bus;

        public EditTaskHandler(ITasksRepo repo, IBus bus)
        {
            _repo = repo;
            _bus = bus;
        }

        public async Task<TaskDTO> Handle(EditTaskCommand request, CancellationToken cancellationToken)
        {

            if (!Enum.TryParse<Priority>(request.TaskToEdit.Priority, true, out Priority priority))
            {
                throw new DomainException($"Unknown priority {request.TaskToEdit.Priority}");
            }

            var taskToEdit = new Core.Models.Task
            {
                Id = request.TaskToEdit.Id,
                Name = request.TaskToEdit.Name,
                CreatedAt = request.TaskToEdit.CreatedAt,
                Deadline = request.TaskToEdit.Deadline,
                Description = request.TaskToEdit.Description,
                Priority = priority
            };

            var existingTask = await _repo.GetByIdAsync(taskToEdit.Id, cancellationToken);

            if (existingTask.Status != taskToEdit.Status)
            {
                taskToEdit.ChangeStatus(existingTask.Status);
            }

            taskToEdit.ChangeStatus(existingTask.Status);

            var changes = ChangesTracker.GetChanges(existingTask, taskToEdit);

            await _repo.UpdateAsync(taskToEdit, cancellationToken);

            if (changes.Any())
            {
                var message = new TaskDataChanged(existingTask.Id, changes, DateTime.UtcNow);

                await _bus.Publish(message, cancellationToken);
            }

            return new TaskDTO
            {
                Name = taskToEdit.Name,
                CreatedAt = taskToEdit.CreatedAt,
                Deadline = taskToEdit.Deadline,
                Description = taskToEdit.Description,
                Priority = taskToEdit.Priority.ToString(),
                Status = taskToEdit.Status.ToString()
            };
        }
    }
}
