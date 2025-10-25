using EmployeesTasksTracker.TasksService.Application.Interfaces;
using QuestPDF.Fluent;
using Shared.Interfaces;
using Shared.Models;

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
                var reportData = await _service.GetTaskReportDataAsync(taskId);

                var reportModel = new TaskReportModel
                {
                    ReportTitle = "Отчёт по задаче",
                    TaskName = reportData.Task.Name,
                    Description = reportData.Task.Description,
                    Deadline = reportData.Task.Deadline,
                    CreatedAt = reportData.Task.CreatedAt,
                    Status = reportData.Task.Status,
                    Priority = reportData.Task.Priority,
                    ProjectName = reportData.ProjectName,
                    TaskGroupName = reportData.TasksGroupName,
                    Performers = reportData.Performers,
                    Observers = reportData.Observers

                };

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
