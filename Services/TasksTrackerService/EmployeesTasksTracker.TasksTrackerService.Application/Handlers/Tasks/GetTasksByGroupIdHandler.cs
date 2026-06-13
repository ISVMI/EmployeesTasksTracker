using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.DTOs;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks
{
    public class GetTasksByGroupIdHandler : IRequestHandler<GetTasksByGroupIdQuery, IEnumerable<TaskForReportDTO>>
    {
        private readonly ITasksRepo _repo;
        private readonly ILogger<GetTasksByGroupIdHandler> _logger;

        public GetTasksByGroupIdHandler(ITasksRepo repo, ILogger<GetTasksByGroupIdHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<IEnumerable<TaskForReportDTO>> Handle(GetTasksByGroupIdQuery request, CancellationToken cancellationToken)
        {
                var tasks = await _repo.GetTasksByGroupId(request.TasksGroupId, cancellationToken);

                var tasksList = new List<TaskForReportDTO>();

                foreach (var task in tasks) 
                {
                    var taskDTO = new TaskForReportDTO
                    {
                        Name = task.Name,
                        Status = task.Status.ToString(),
                        CreatedAt = task.CreatedAt.ToString("dd.MM.yyyy HH:mm"),
                        Deadline = task.Deadline.ToString("dd.MM.yyyy HH:mm"),
                        Description = task.Description,
                        Priority = task.Priority.ToString()
                    };

                    tasksList.Add(taskDTO);
                }

            _logger.LogInformation("Successfully got {totalCount} tasks by tasks group with id {tasksGroupId}", tasksList.Count(), request.TasksGroupId);

            return tasksList;
        }
    }
}
