using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using EmployeesTasksTracker.TasksTrackerService.Infrastructure.Data;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Infrastructure.Repositories
{
    public class TaskEmployeeRepo : ITaskEmployeeRepo
    {
        private readonly TasksTrackerContext _context;

        public TaskEmployeeRepo(TasksTrackerContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task AddEmployeeAsync(Guid employeeId, Guid taskId, RoleInTask roleInTask, CancellationToken token = default)
        {
            var employeeToAdd = new TaskEmployee
            {
                EmployeeId = employeeId,
                TaskId = taskId,
                EmployeeRoleInTask = roleInTask
            };

            await _context.TaskEmployees.AddAsync(employeeToAdd, token);

            await _context.SaveChangesAsync(token);
        }

        public async Task<IEnumerable<TaskEmployee>> GetAllById(Guid? taskId = null, Guid? employeeId = null, CancellationToken token = default)
        {
            if (taskId == null && employeeId == null)
            {
                throw new DomainException($"Both parameters were null");
            }

            if (employeeId == null)
            {
                return _context.TaskEmployees.Where(te => te.TaskId == taskId).ToList();
            }

            if (taskId == null)
            {
                return _context.TaskEmployees.Where(te => te.EmployeeId == employeeId).ToList();
            }

            return _context.TaskEmployees.Where(te => te.TaskId == taskId && te.EmployeeId == employeeId).ToList();
        }
    }
}
