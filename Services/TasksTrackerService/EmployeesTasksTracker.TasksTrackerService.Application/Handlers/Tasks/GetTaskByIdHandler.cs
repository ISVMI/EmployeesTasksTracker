using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks
{
    public class GetTaskByIdHandler : IRequestHandler<GetTaskByIdQuery, TaskDTO>
    {
        private readonly ITasksRepo _repo;

        public GetTaskByIdHandler(ITasksRepo repo)
        {
            _repo = repo;
        }
        public async Task<TaskDTO> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = await _repo.GetByIdAsync(request.Id, cancellationToken);

            return new TaskDTO
            {
                Name = task.Name,
                CreatedAt = task.CreatedAt,
                Deadline = task.Deadline,
                Description = task.Description,
                Priority = task.Priority.ToString(),
                Status = task.Status.ToString(),
            };
        }
    }
}
