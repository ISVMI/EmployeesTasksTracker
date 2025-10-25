using EmployeesTasksTracker.TasksService.Application.Interfaces;
using EmployeesTasksTracker.TasksService.Core.Interfaces;
using Shared.DTOs;
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

        public async Task<TaskReportData> GetTaskReportDataAsync(Guid id, CancellationToken cancellationToken = default)
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

                return new TaskReportData
                {
                    Task = new TaskForReportDTO
                    {
                        Name = task.Name,
                        Description = task.Description,
                        CreatedAt = task.CreatedAt.ToString("dd.MM.yyyy HH:mm"),
                        Deadline = task.Deadline.ToString("dd.MM.yyyy HH:mm"),
                        Status = Translate(task.Status.ToString()),
                        Priority = Translate(task.Priority.ToString())
                    },
                    ProjectName = project,
                    TasksGroupName = tasksGroup,
                    Performers = performers,
                    Observers = observers
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get task report data {ex.Message}");
            }
        }

        private static string Translate(string word)
        {
            switch (word)
            {
                case "Low":
                    {
                        return "Низкий";
                    }
                case "Medium":
                    {
                        return "Средний";
                    }
                case "High":
                    {
                        return "Высокий";
                    }
                case "Critical":
                    {
                        return "Критический";
                    }
                case "Blocker":
                    {
                        return "Блокер";
                    }
                case "Backlog":
                    {
                        return "Бэклог";
                    }
                case "Current":
                    {
                        return "Текущая";
                    }
                case "Active":
                    {
                        return "Активная";
                    }
                case "Testing":
                    {
                        return "Тестируется";
                    }
                case "Completed":
                    {
                        return "Завершена";
                    }
                case "Canceled":
                    {
                        return "Отменена";
                    }
                default:
                    {
                        return "Неизвестно";
                    }
            }
        }
    }
}
