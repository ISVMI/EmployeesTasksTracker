using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks
{
    public class GetAllTasksHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskDTO>>
    {
        private readonly ITasksRepo _repo;

        public GetAllTasksHandler(ITasksRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<TaskDTO>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _repo.GetAllAsync(request.EmployeeId, request.TasksGroupId, request.ProjectId, cancellationToken);

            var tasksDtoList = new List<TaskDTO>();

            foreach (var task in tasks)
            {
                tasksDtoList.Add(new TaskDTO
                {
                    Name = task.Name,
                    CreatedAt = task.CreatedAt,
                    Deadline = task.Deadline,
                    Description = task.Description,
                    Priority = task.Priority.ToString(),
                    Status = task.Status.ToString(),
                });
            }
            
            return tasksDtoList;
        }
    }
}
