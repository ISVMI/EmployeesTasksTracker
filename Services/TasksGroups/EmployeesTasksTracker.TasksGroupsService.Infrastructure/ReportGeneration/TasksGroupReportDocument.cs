using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Shared.DTOs;
using Shared.Models;

namespace EmployeesTasksTracker.TasksGroupsService.Infrastructure.ReportGeneration
{
    public class TasksGroupReportDocument : IDocument
    {
        private readonly TasksGroupReportModel _model;

        public TasksGroupReportDocument(TasksGroupReportModel model)
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
                column.Item().AlignCenter().Text(_model.ReportTitle ??= $"Отчёт по группе задач")
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

                column.Item().Element(ComposeTasksGroupInfo);

                if (_model.Tasks.Any())
                {
                    column.Item().Element(ComposeTasks);
                }
            });
        }

        private void ComposeTasksGroupInfo(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(8);

                column.Item().Text("Информация о группе задач:").SemiBold().FontSize(14);

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
                        table.Cell().Border(1).PaddingLeft(5).Text(_model.Name).LineHeight(2);
                        table.Cell().Border(1).PaddingLeft(5).Text("Проект").SemiBold().LineHeight(2);
                        table.Cell().Border(1).PaddingLeft(5).Text(_model.ProjectName).LineHeight(2);
                    });
                });

            });
        }

        private void ComposeTasks(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Text("Задачи").SemiBold().FontSize(14);

                foreach (var task in _model.Tasks)
                {
                    column.Item().Padding(8).Element(c => ComposeTasksTable(c, task));
                }
            });
        }

        private void ComposeTasksTable(IContainer container, TaskForReportDTO task)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(3);
                });
                table.Cell().Border(1).PaddingLeft(5).Text("Название").SemiBold().LineHeight(2);
                table.Cell().Border(1).PaddingLeft(5).Text(task.Name).LineHeight(2);
                table.Cell().Border(1).PaddingLeft(5).Text("Создана").SemiBold().LineHeight(2);
                table.Cell().Border(1).PaddingLeft(5).Text(task.CreatedAt).LineHeight(2);
                table.Cell().Border(1).PaddingLeft(5).Text("Дэдлайн").SemiBold().LineHeight(2);
                table.Cell().Border(1).PaddingLeft(2).Text(task.Deadline).LineHeight(2);
                table.Cell().Border(1).PaddingLeft(5).Text("Статус").SemiBold().LineHeight(2);
                table.Cell().Border(1).PaddingLeft(5).Text(task.Status).LineHeight(2);
                table.Cell().Border(1).PaddingLeft(5).Text("Приоритет").SemiBold().LineHeight(2);
                table.Cell().Border(1).PaddingLeft(5).Text(task.Priority).LineHeight(2);
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
