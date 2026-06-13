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

            await _cache.RemoveAsync($"task:{request.TaskToEdit.Id}", cancellationToken);

            if (!Enum.TryParse<Priority>(request.TaskToEdit.Priority, true, out Priority priority))
            {
                throw new DomainException($"Unknown priority {request.TaskToEdit.Priority}");
            }

            var taskToEdit = await _repo.GetByIdAsync(request.TaskToEdit.Id, cancellationToken)
                ?? throw new NotFoundException("task", request.TaskToEdit.Id);

            taskToEdit.Name = request.TaskToEdit.Name;
            taskToEdit.CreatedAt = request.TaskToEdit.CreatedAt;
            taskToEdit.Deadline = request.TaskToEdit.Deadline;
            taskToEdit.Description = request.TaskToEdit.Description;
            taskToEdit.Priority = priority;

            var existingTask = await _repo.GetByIdAsync(taskToEdit.Id, cancellationToken);

            if (existingTask.Status != taskToEdit.Status)
            {
                taskToEdit.ChangeStatus(existingTask.Status);
            }

            taskToEdit.ChangeStatus(existingTask.Status);

            var changes = ChangesTracker.GetChanges(existingTask, taskToEdit);

            var editedTask = await _repo.UpdateAsync(taskToEdit, cancellationToken) ?? throw new NotFoundException("task", request.TaskToEdit.Id);

            if (changes.Any())
            {
                var message = new TaskDataChanged(existingTask.Id, changes, DateTime.UtcNow);

                await _bus.Publish(message, cancellationToken);
            }

            _logger.LogInformation("Successfully edited task with id {taskId}", editedTask.Id);

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
