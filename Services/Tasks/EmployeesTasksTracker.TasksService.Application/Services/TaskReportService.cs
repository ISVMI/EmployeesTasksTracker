using EmployeesTasksTracker.TasksService.Application.Interfaces;
using EmployeesTasksTracker.TasksService.Core.Interfaces;
using Shared.DTOs;
using Shared.Methods;
using Shared.Models;

namespace EmployeesTasksTracker.TasksService.Application.Services
{
    public class TaskReportService : ITaskReportService
    {
        private readonly ITasksRepo _repo;
        private readonly IProjectsClient _projectsClient;
        private readonly ITasksGroupsClient _tasksGroupClient;
        private readonly IEmployeesClient _employeesClient;

        public TaskReportService(ITasksRepo repo, IProjectsClient projectsClient, ITasksGroupsClient tasksGroupsClient, IEmployeesClient employeesClient)
        {
            _repo = repo;
            _projectsClient = projectsClient;
            _tasksGroupClient = tasksGroupsClient;
            _employeesClient = employeesClient;
        }

        public async Task<TaskReportModel> GetTaskReportDataAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var task = await _repo.GetByIdAsync(id, cancellationToken);

                var project = await _projectsClient.GetProjectName(task.Project, cancellationToken);

                var tasksGroup = await _tasksGroupClient.GetTasksGroupName(task.TasksGroup, cancellationToken);

                var performers = new List<EmployeeForReportDTO>();

                for (int i = 0; i < task.Performers.Count; i++)
                {
                    var performer = await _employeesClient.GetEmployeeInfo(task.Performers[i], cancellationToken);

                    performers.Add(performer);
                }

                var observers = new List<EmployeeForReportDTO>();

                for (int i = 0; i < task.Observers.Count; i++)
                {
                    var observer = await _employeesClient.GetEmployeeInfo(task.Observers[i], cancellationToken);

                    observers.Add(observer);
                }

                return new TaskReportModel
                {
                    ReportTitle = "Отчёт о задаче",
                    TaskName = task.Name,
                    Description = task.Description,
                    Deadline = task.Deadline.ToString("dd.MM.yyyy HH:mm"),
                    CreatedAt = task.CreatedAt.ToString("dd.MM.yyyy HH:mm"),
                    Status = EnumsHumanizer.Translate(task.Status.ToString()),
                    Priority = EnumsHumanizer.Translate(task.Priority.ToString()),
                    ProjectName = project,
                    TaskGroupName = tasksGroup,
                    Performers = performers,
                    Observers = observers
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get task report data {ex.Message}");
            }
        }
    }
}
