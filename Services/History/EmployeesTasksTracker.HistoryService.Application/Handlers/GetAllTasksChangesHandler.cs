using EmployeesTasksTracker.HistoryService.Application.DTOs;
using EmployeesTasksTracker.HistoryService.Application.Queries;
using EmployeesTasksTracker.HistoryService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeesTasksTracker.HistoryService.Application.Handlers
{
    internal class GetAllTasksChangesHandler : IRequestHandler<GetAllTasksChangesQuery, IEnumerable<TaskChangesDTO>>
    {
        private readonly ITaskChangesRepo _repo;
        private readonly ILogger _logger;

        public GetAllTasksChangesHandler(ITaskChangesRepo repo, ILogger logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<IEnumerable<TaskChangesDTO>> Handle(GetAllTasksChangesQuery request, CancellationToken cancellationToken)
        {
            var taskChanges = await _repo.GetAllChanges(cancellationToken);

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
    }
}
