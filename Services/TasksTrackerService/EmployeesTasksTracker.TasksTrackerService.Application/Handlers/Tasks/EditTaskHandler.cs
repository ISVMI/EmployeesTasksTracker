using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using Shared.Messages;
using Shared.Methods;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks
{
    public class EditTaskHandler : IRequestHandler<EditTaskCommand, TaskDTO>
    {
        private readonly ITasksRepo _repo;
        private readonly IBus _bus;
        private readonly HybridCache _cache;
        private readonly ILogger<EditTaskHandler> _logger;

        public EditTaskHandler(ITasksRepo repo, IBus bus, HybridCache cache, ILogger<EditTaskHandler> logger)
        {
            _repo = repo;
            _bus = bus;
            _cache = cache;
            _logger = logger;
        }

        public async Task<TaskDTO> Handle(EditTaskCommand request, CancellationToken cancellationToken)
        {
            //Removing task that is being edited from the cache
            await _cache.RemoveAsync($"task:{request.TaskToEdit.Id}", cancellationToken);

            //Checking if the given priority matches existing enum value
            if (!Enum.TryParse<Priority>(request.TaskToEdit.Priority, true, out Priority priority))
            {
                throw new DomainException($"Unknown priority {request.TaskToEdit.Priority}");
            }

            //Getting task to edit from repository
            var taskToEdit = await _repo.GetByIdAsync(request.TaskToEdit.Id, cancellationToken)
                ?? throw new NotFoundException("task", request.TaskToEdit.Id);

            //Creating a "snapshot" of an existing task
            var existingTask = new Core.Models.Task
            {
                Id = taskToEdit.Id,
                Name = taskToEdit.Name,
                CreatedAt = taskToEdit.CreatedAt,
                Deadline = taskToEdit.Deadline,
                Description = taskToEdit.Description,
                Priority = taskToEdit.Priority
            };

            if (existingTask.Status != taskToEdit.Status)
            {
                existingTask.ChangeStatus(taskToEdit.Status);
            }

            //Changing values of task to edit
            taskToEdit.Name = request.TaskToEdit.Name;
            taskToEdit.CreatedAt = request.TaskToEdit.CreatedAt;
            taskToEdit.Deadline = request.TaskToEdit.Deadline;
            taskToEdit.Description = request.TaskToEdit.Description;
            taskToEdit.Priority = priority;

            //Getting changes that is being made
            var changes = ChangesTracker.GetChanges(existingTask, taskToEdit);

            //Updating entity in database
            var editedTask = await _repo.UpdateAsync(taskToEdit, cancellationToken) ?? throw new NotFoundException("task", request.TaskToEdit.Id);

            //Publishing message of changes that took place if there is any
            if (changes.Any())
            {
                var message = new TaskDataChanged(existingTask.Id, changes, DateTime.UtcNow);

                await _bus.Publish(message, cancellationToken);
            }


            //Logging a fact of successfull editing
            _logger.LogInformation("Successfully edited task with id {taskId}", editedTask.Id);

            return new TaskDTO
            {
                Name = editedTask.Name,
                CreatedAt = editedTask.CreatedAt,
                Deadline = editedTask.Deadline,
                Description = editedTask.Description,
                Priority = editedTask.Priority.ToString(),
                Status = editedTask.Status.ToString()
            };
        }
    }
}
