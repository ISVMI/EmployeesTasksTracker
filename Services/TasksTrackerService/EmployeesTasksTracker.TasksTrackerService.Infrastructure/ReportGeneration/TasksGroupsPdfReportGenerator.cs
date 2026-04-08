using EmployeesTasksTracker.TasksTrackerService.Application.Interfaces;
using QuestPDF.Fluent;
using Shared.Interfaces;

namespace EmployeesTasksTracker.TasksTrackerService.Infrastructure.ReportGeneration
{
    public class TasksGroupsPdfReportGenerator : IPdfReportGenerator
    {
        private readonly ITasksGroupReportService _service;

        public TasksGroupsPdfReportGenerator(ITasksGroupReportService service)
        {
            _service = service;
        }

        public async Task<byte[]> GenerateReportAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var reportModel = await _service.GetTasksGroupReportDataAsync(id);

                var document = new TasksGroupReportDocument(reportModel);

                return document.GeneratePdf();
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not create report {ex.Message}");
            }
        }
    }
}
