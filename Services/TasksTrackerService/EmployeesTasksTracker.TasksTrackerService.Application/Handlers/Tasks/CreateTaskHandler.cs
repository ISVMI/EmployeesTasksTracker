using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;

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
            var newTask = new Core.Models.Task
            {
                Name = request.Task.Name,
                CreatedAt = request.Task.CreatedAt,
                Deadline = request.Task.Deadline,
                Description = request.Task.Description,
                Priority = Priority.Low, //request.Task.Priority,
            };

            // newTask.ChangeStatus(request.Task.Status);

            await _repo.CreateAsync(newTask, cancellationToken);

            return newTask.Id;
        }
    }
}
