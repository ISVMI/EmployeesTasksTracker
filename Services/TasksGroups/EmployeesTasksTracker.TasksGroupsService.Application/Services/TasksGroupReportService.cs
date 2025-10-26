using EmployeesTasksTracker.TasksGroupsService.Application.Interfaces;
using EmployeesTasksTracker.TasksGroupsService.Core.Interfaces;
using Shared.Models;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Services
{
    public class TasksGroupReportService : ITasksGroupReportService
    {
        private readonly ITasksGroupsRepo _repo;
        private readonly ITasksClient _tasksClient;
        private readonly IProjectsClient _projectsClient;

        public TasksGroupReportService(ITasksGroupsRepo repo, ITasksClient tasksClient, IProjectsClient projectsClient)
        {
            _repo = repo;
            _tasksClient = tasksClient;
            _projectsClient = projectsClient;
        }

        public async Task<TasksGroupReportModel> GetTasksGroupReportDataAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var tasksGroup = await _repo.GetByIdAsync(id, cancellationToken);

                var projectId = await _tasksClient.GetProjectId(id, cancellationToken);

                var tasks = await _tasksClient.GetTasks(id, cancellationToken);

                var projectName = await _projectsClient.GetProjectName(projectId, cancellationToken);

                return new TasksGroupReportModel
                {
                    ReportTitle = "Отчёт о группе задач",
                    Name = tasksGroup.Name,
                    ProjectName = projectName,
                    Tasks = tasks.ToList()
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get tasks group report data {ex.Message}");
            }
        }
    }
}
