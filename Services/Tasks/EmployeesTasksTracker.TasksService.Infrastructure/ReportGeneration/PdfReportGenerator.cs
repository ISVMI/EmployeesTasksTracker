using EmployeesTasksTracker.TasksService.Application.Interfaces;
using QuestPDF.Fluent;
using Shared.Interfaces;

namespace EmployeesTasksTracker.TasksService.Infrastructure.ReportGeneration
{
    public class PdfReportGenerator : IPdfReportGenerator
    {
        private readonly ITaskReportService _service;

        public PdfReportGenerator(ITaskReportService service)
        {
            _service = service;
        }

        public async Task<byte[]> GenerateReportAsync(Guid taskId, CancellationToken cancellationToken = default)
        {
            try
            {
                var reportModel = await _service.GetTaskReportDataAsync(taskId);

                var document = new TaskReportDocument(reportModel);

                return document.GeneratePdf();
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not create report {ex.Message}");
            }
        }
    }
}
