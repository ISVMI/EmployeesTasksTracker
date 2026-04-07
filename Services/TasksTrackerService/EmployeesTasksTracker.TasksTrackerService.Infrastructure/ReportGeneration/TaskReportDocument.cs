using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Shared.DTOs;
using Shared.Models;

namespace EmployeesTasksTracker.TasksService.Infrastructure.ReportGeneration
{
    public class TaskReportDocument : IDocument
    {
        private readonly TaskReportModel _model;

        public TaskReportDocument(TaskReportModel model)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            _model = model;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().Element(ComposeFooter);
            });
        }

        private void ComposeHeader(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().AlignCenter().Text(_model.ReportTitle ??= $"Отчёт по задаче")
                .FontSize(20).SemiBold();

                column.Item().AlignCenter().Text($"Создан {_model.GeneratedAt:dd.MM.yyyy HH.mm}")
                .FontSize(12).FontColor(Colors.Grey.Medium);

                column.Item().PaddingBottom(10).LineHorizontal(1);

            });
        }

        private void ComposeContent(IContainer container)
        {
            container.PaddingVertical(10).Column(column =>
            {
                column.Spacing(15);

                column.Item().Element(ComposeTaskInfo);

                if (_model.Performers.Any())
                {
                    column.Item().Element(ComposePerformers);
                }

                if (_model.Observers.Any())
                {
                    column.Item().Element(ComposeObservers);
                }
            });
        }

        private void ComposeTaskInfo(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(8);

                column.Item().Text("Информация о задаче:").SemiBold().FontSize(14);

                column.Item().Element(element =>
                {
                    element.Table(table =>
                    {

                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(3);
                        });

                        table.Cell().Border(1).PaddingLeft(5).Text("Название").SemiBold().LineHeight(2);
                        table.Cell().Border(1).PaddingLeft(5).Text(_model.TaskName).LineHeight(2);
                        table.Cell().Border(1).PaddingLeft(5).Text("Создана").SemiBold().LineHeight(2);
                        table.Cell().Border(1).PaddingLeft(5).Text(_model.CreatedAt).LineHeight(2);
                        table.Cell().Border(1).PaddingLeft(5).Text("Дэдлайн").SemiBold().LineHeight(2);
                        table.Cell().Border(1).PaddingLeft(2).Text(_model.Deadline).LineHeight(2);
                        table.Cell().Border(1).PaddingLeft(5).Text("Статус").SemiBold().LineHeight(2);
                        table.Cell().Border(1).PaddingLeft(5).Text(_model.Status).LineHeight(2);
                        table.Cell().Border(1).PaddingLeft(5).Text("Приоритет").SemiBold().LineHeight(2);
                        table.Cell().Border(1).PaddingLeft(5).Text(_model.Priority).LineHeight(2);
                        table.Cell().Border(1).PaddingLeft(5).Text("Проект").SemiBold().LineHeight(2);
                        table.Cell().Border(1).PaddingLeft(5).Text(_model.ProjectName).LineHeight(2);
                        table.Cell().Border(1).PaddingLeft(5).Text("Группа задач").SemiBold().LineHeight(2);
                        table.Cell().Border(1).PaddingLeft(5).Text(_model.TaskGroupName).LineHeight(2);
                    });
                });

                if (!string.IsNullOrEmpty(_model.Description))
                {
                    column.Item().PaddingTop(10).Text("Описание").SemiBold();
                    column.Item().Text(_model.Description);
                }

            });
        }

        private void ComposePerformers(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Text("Исполнители").SemiBold().FontSize(14);
                column.Item().Element(c => ComposeEmployeesTable(c, _model.Performers));
            });
        }

        private void ComposeObservers(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Text("Наблюдатели").SemiBold().FontSize(14);
                column.Item().Element(c => ComposeEmployeesTable(c, _model.Observers));
            });
        }

        private void ComposeEmployeesTable(IContainer container, List<EmployeeForReportDTO> employees)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(3);
                    columns.RelativeColumn(2);
                });

                table.Header(header =>
                {
                    header.Cell().Border(1).Padding(8).Text("ФИО").SemiBold();
                    header.Cell().Border(1).Padding(8).Text("Роль").SemiBold();
                });

                foreach (var employee in employees)
                {
                    var fullName = $"{employee.Surname} {employee.Name} {employee.Patronymic}";
                    table.Cell().Border(1).Padding(8).Text(fullName);
                    table.Cell().Border(1).Padding(8).Text(employee.Role);
                }

            });
        }

        private void ComposeFooter(IContainer container)
        {

            container.AlignCenter().Text(text =>
            {
                text.Span("Страница ");
                text.CurrentPageNumber();
                text.Span(" из ");
                text.TotalPages();
            });
        }
    }
}
