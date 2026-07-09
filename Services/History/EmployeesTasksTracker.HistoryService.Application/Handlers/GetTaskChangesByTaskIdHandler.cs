using EmployeesTasksTracker.HistoryService.Application.DTOs;
using EmployeesTasksTracker.HistoryService.Application.Queries;
using EmployeesTasksTracker.HistoryService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeesTasksTracker.HistoryService.Application.Handlers
{
    public class GetTaskChangesByTaskIdHandler : IRequestHandler<GetTaskChangesByTaskIdQuery, IEnumerable<TaskChangesDTO>>
    {
        private readonly ITaskChangesRepo _repo;
        private readonly ILogger _logger;

        public GetTaskChangesByTaskIdHandler(ITaskChangesRepo repo, ILogger logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<IEnumerable<TaskChangesDTO>> Handle(GetTaskChangesByTaskIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var taskChanges = await _repo.GetChangesByTaskId(request.TaskId, cancellationToken);

                var taskChangesDtoList = new List<TaskChangesDTO>();

                Parallel.ForEach(taskChanges, task =>
                {
                    taskChangesDtoList.Add(new TaskChangesDTO
                    {
                        TaskId = task.TaskId,
                        ChangedAt = task.ChangedAt.ToString($"dd.MM.yyyy HH:mm:ss"),
                        Changes = task.Changes
                    });
                });

                _logger.LogInformation("Successfully got {changesCount} records", taskChangesDtoList.Count);

                return taskChangesDtoList;
            }
            catch (Exception ex)
            {
                var message = $"Could not get changes by task id {ex.Message}";

                _logger.LogError(message);

                throw new Exception(message);
            }
        }
    }
}
