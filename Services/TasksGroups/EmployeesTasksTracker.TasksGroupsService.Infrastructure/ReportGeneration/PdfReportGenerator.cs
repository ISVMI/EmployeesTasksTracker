using EmployeesTasksTracker.TasksGroupsService.Application.Interfaces;
using QuestPDF.Fluent;
using Shared.Interfaces;

namespace EmployeesTasksTracker.TasksGroupsService.Infrastructure.ReportGeneration
{
    public class PdfReportGenerator : IPdfReportGenerator
    {
        private readonly ITasksGroupReportService _service;

        public PdfReportGenerator(ITasksGroupReportService service)
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
