using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks
{
    public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, Guid>
    {
        private readonly ITasksRepo _repo;

        public CreateTaskHandler(ITasksRepo repo)
        {
            _repo = repo;
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

            await _repo.CreateAsync(newTask, cancellationToken);

            return newTask.Id;
        }
    }
}
