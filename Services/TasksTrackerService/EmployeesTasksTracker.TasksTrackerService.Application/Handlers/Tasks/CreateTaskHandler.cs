using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MassTransit;
using MediatR;
using Shared.DTOs;
using Shared.Exceptions;
using Shared.Messages;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks
{
    public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, Guid>
    {
        private readonly ITasksRepo _repo;
        private readonly IBus _bus;

        public CreateTaskHandler(ITasksRepo repo, IBus bus)
        {
            _repo = repo;
            _bus = bus;
        }

        public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {

            if (!Enum.TryParse<Priority>(request.Task.Priority, true, out Priority priority))
            {
                throw new DomainException($"Unknown priority {request.Task.Priority}");
            }

            if (!Enum.TryParse<Status>(request.Task.Status, true, out Status status))
            {
                throw new DomainException($"Unknown status {request.Task.Status}");
            }

            var newTask = new Core.Models.Task
            {
                Name = request.Task.Name,
                CreatedAt = request.Task.CreatedAt,
                Deadline = request.Task.Deadline,
                Description = request.Task.Description,
                Priority = priority
            };

            if (newTask.Status != status)
            {
                newTask.ChangeStatus(status);
            }

            var taskId = await _repo.CreateAsync(newTask, cancellationToken);

            var message = new TaskCreated
            {
                TaskId = taskId,
                Name = request.Task.Name,
                CreatedAt = request.Task.CreatedAt.ToString($"dd.MM.yyyy HH:mm:ss"),
                Deadline = request.Task.Deadline.ToString($"dd.MM.yyyy HH:mm:ss"),
                Description = request.Task.Description,
                Priority = request.Task.Priority,
                Status = request.Task.Status
            };

            return newTask.Id;
        }
    }
}
