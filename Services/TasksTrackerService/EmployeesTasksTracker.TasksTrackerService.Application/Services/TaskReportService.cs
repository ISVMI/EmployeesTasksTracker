using EmployeesTasksTracker.TasksTrackerService.Application.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using Shared.DTOs;
using Shared.Exceptions;
using Shared.Methods;
using Shared.Models;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Services
{
    public class TaskReportService : ITaskReportService
    {
        private readonly ITasksRepo _repo;
        private readonly IProjectsRepo _projectsRepo;
        private readonly ITasksGroupsRepo _tasksGroupRepo;
        private readonly ITaskEmployeeRepo _taskEmployeeRepo;
        private readonly IEmployeesRepo _employeesRepo;

        public TaskReportService(
            ITasksRepo repo,
            IProjectsRepo projectsRepo,
            ITasksGroupsRepo tasksGroupsRepo,
            ITaskEmployeeRepo taskEmployeeRepo,
            IEmployeesRepo employeesRepo)
        {
            _repo = repo;
            _projectsRepo = projectsRepo;
            _tasksGroupRepo = tasksGroupsRepo;
            _taskEmployeeRepo = taskEmployeeRepo;
            _employeesRepo = employeesRepo;
        }

        public async Task<TaskReportModel> GetTaskReportDataAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var task = await _repo.GetByIdAsync(id, cancellationToken);

                var project = await _projectsRepo.GetByIdAsync(task.ProjectId, cancellationToken);

                var tasksGroup = await _tasksGroupRepo.GetByIdAsync(task.TasksGroupId, cancellationToken);

                var tasksEmployees = await _taskEmployeeRepo.GetAllById(task.Id, null, cancellationToken);

                var performers = new List<Employee>();

                var observers = new List<Employee>();

                var performersForReport = new List<EmployeeForReportDTO>();

                var observersForReport = new List<EmployeeForReportDTO>();

                foreach (var relation in tasksEmployees)
                {
                    var employee = await _employeesRepo.GetByIdAsync(relation.EmployeeId, cancellationToken);

                    if (relation.EmployeeRoleInTask == Core.Enums.RoleInTask.Performer)
                    {
                        performers.Add(employee);
                    }
                    else
                    {
                        observers.Add(employee);
                    }
                }
                ;

                AddEmployeesForReport(performers, performersForReport);

                AddEmployeesForReport(observers, observersForReport);

                return new TaskReportModel
                {
                    ReportTitle = "Отчёт о задаче",
                    TaskName = task.Name,
                    Description = task.Description,
                    Deadline = task.Deadline.ToString("dd.MM.yyyy HH:mm"),
                    CreatedAt = task.CreatedAt.ToString("dd.MM.yyyy HH:mm"),
                    Status = EnumsHumanizer.Translate(task.Status.ToString()),
                    Priority = EnumsHumanizer.Translate(task.Priority.ToString()),
                    ProjectName = project.Name,
                    TaskGroupName = tasksGroup.Name,
                    Performers = performersForReport,
                    Observers = observersForReport
                };
            }
            catch (Exception ex)
            {
                throw new DomainException($"Could not get task report data {ex.Message}");
            }
        }

        private static void AddEmployeesForReport(List<Employee> employees, List<EmployeeForReportDTO> employeesForReport)
        {
            Parallel.ForEach(employees, (employee) =>
            {
                employeesForReport.Add(new EmployeeForReportDTO
                {
                    Name = employee.Name,
                    Surname = employee.Surname,
                    Patronymic = employee.Patronymic,
                    Role = EnumsHumanizer.Translate(employee.Role.ToString())
                });
            });

        }
    }
}
