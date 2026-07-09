using EmployeesTasksTracker.HistoryService.Application.Commands;
using EmployeesTasksTracker.HistoryService.Core.Interfaces;
using EmployeesTasksTracker.HistoryService.Core.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeesTasksTracker.HistoryService.Application.Handlers
{
    public class CreateTaskChangesRecordHandler : IRequestHandler<CreateTaskChangesRecordCommand, Guid>
    {
        private readonly ITaskChangesRepo _repo;
        private readonly ILogger<CreateTaskChangesRecordHandler> _logger;

        public CreateTaskChangesRecordHandler(ITaskChangesRepo repo, ILogger<CreateTaskChangesRecordHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateTaskChangesRecordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                DateTime.TryParseExact(request.TaskChanges.ChangedAt, $"dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, 0, out var changedAt);

                var taskChanges = new TaskChanges
                {
                    TaskId = request.TaskChanges.TaskId,
                    ChangedAt = changedAt,
                    Changes = request.TaskChanges.Changes
                };

                var taskChangesId = await _repo.CreateTaskChangesRecord(taskChanges, cancellationToken);

                return taskChangesId;
            }
            catch (Exception ex)
            {
                var message = $"Could not create record of task changes {ex.Message}";

                _logger.LogError(message);

                throw new Exception(message);

            }
        }
    }
}
