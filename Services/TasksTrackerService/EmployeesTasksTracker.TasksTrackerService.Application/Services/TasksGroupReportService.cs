using EmployeesTasksTracker.TasksTrackerService.Application.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using Shared.DTOs;
using Shared.Exceptions;
using Shared.Methods;
using Shared.Models;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Services
{
    public class TasksGroupReportService : ITasksGroupReportService
    {
        private readonly ITasksGroupsRepo _repo;
        private readonly ITasksRepo _tasksRepo;
        private readonly IProjectsRepo _projectsRepo;

        public TasksGroupReportService(ITasksGroupsRepo repo, ITasksRepo tasksRepo, IProjectsRepo projectsRepo)
        {
            _repo = repo;
            _tasksRepo = tasksRepo;
            _projectsRepo = projectsRepo;
        }

        public async Task<TasksGroupReportModel> GetTasksGroupReportDataAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var tasksGroup = await _repo.GetByIdAsync(id, cancellationToken);

                var projectId = await _tasksRepo.GetProjectId(id, cancellationToken);

                var tasks = await _tasksRepo.GetAllAsync(null, id, null, cancellationToken);

                var task = tasksGroup.Tasks.FirstOrDefault() ?? throw new DomainException($"Tasks group {tasksGroup.Name} doesn't contain any tasks!");

                var project = await _projectsRepo.GetByIdAsync(task.ProjectId, cancellationToken);

                var tasksForReport = new List<TaskForReportDTO>();

                Parallel.ForEach(tasks, (task) =>
                {
                    tasksForReport.Add(new TaskForReportDTO
                    {
                        Name = task.Name,
                        CreatedAt = task.CreatedAt.ToString("dd.MM.yyyy HH:mm"),
                        Deadline = task.Deadline.ToString("dd.MM.yyyy HH:mm"),
                        Description = task.Description,
                        Status = EnumsHumanizer.Translate(task.Status.ToString()),
                        Priority = EnumsHumanizer.Translate(task.Priority.ToString())
                    });
                });

                return new TasksGroupReportModel
                {
                    ReportTitle = "Отчёт о группе задач",
                    Name = tasksGroup.Name,
                    ProjectName = project.Name,
                    Tasks = tasksForReport
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get tasks group report data {ex.Message}");
            }
        }
    }
}
