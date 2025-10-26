using EmployeesTasksTracker.TasksService.Application.Queries;
using EmployeesTasksTracker.TasksService.Core.Interfaces;
using MediatR;
using Shared.DTOs;

namespace EmployeesTasksTracker.TasksService.Application.Handlers
{
    public class GetTasksByGroupIdHandler : IRequestHandler<GetTasksByGroupIdQuery, IEnumerable<TaskForReportDTO>>
    {
        private readonly ITasksRepo _repo;

        public GetTasksByGroupIdHandler(ITasksRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<TaskForReportDTO>> Handle(GetTasksByGroupIdQuery request, CancellationToken cancellationToken)
        {
            try
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

                return tasksList;
            }
            catch (Exception ex) 
            {
                throw new Exception($"Could not get tasks by group id: {ex.Message}");
            }
        }
    }
}
