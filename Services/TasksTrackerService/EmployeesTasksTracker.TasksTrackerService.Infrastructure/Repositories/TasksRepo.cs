using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Infrastructure.Repositories
{
    public class TasksRepo : ITasksRepo
    {
        private readonly TasksTrackerContext _context;

        public TasksRepo(TasksTrackerContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(Core.Models.Task task, CancellationToken token = default)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task), "Given task was null!");
            }

            if (task.Status != Status.Backlog && task.Status != Status.Current)
            {
                throw new DomainException($"Could not create task with the given status - {task.Status}");
            }

            await _context.Tasks.AddAsync(task, token);
            await _context.SaveChangesAsync(token);
            return task.Id;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
            var taskToDelete = await GetByIdAsync(id, token);

            if (taskToDelete is null)
            {
                throw new NotFoundException("task", id);
            }

            else if (taskToDelete.Status == Status.Active ||
                taskToDelete.Status == Status.Testing)
            {
                throw new DomainException($"Could not delete task with status - {taskToDelete.Status}");
            }

            _context.Tasks.Remove(taskToDelete);
            await _context.SaveChangesAsync(token);
            return true;
        }
        public async Task<IEnumerable<Core.Models.Task>> GetAllAsync(CancellationToken token = default)
        {
            var query = _context.Tasks.AsNoTracking();

            var tasks = await query.ToListAsync(token);

            return tasks;
        }

        public async Task<IEnumerable<Core.Models.Task>> GetAllFilteredAsync(
            Guid? employeeId = null,
            Guid? tasksGroupId = null,
            Guid? projectId = null,
            CancellationToken token = default)
        {
            var query = _context.Tasks
                .Include(t => t.TasksGroup)
                .Include(t => t.Project)
                .Include(t => t.TaskEmployees)
                .AsNoTracking()
                .AsSplitQuery();

            if (employeeId.HasValue && employeeId != Guid.Empty)
            {
                query = query.Where(t => t.TaskEmployees.Any(te => te.EmployeeId == employeeId.Value));
            }

            if (tasksGroupId.HasValue && tasksGroupId != Guid.Empty)
            {
                query = query.Where(t => t.TasksGroupId == tasksGroupId.Value);
            }

            if (projectId.HasValue && projectId != Guid.Empty)
            {
                query = query.Where(t => t.ProjectId == projectId.Value);
            }

            var tasks = await query.ToListAsync(token);

            return tasks;
        }

        public async Task<IEnumerable<Guid>> GetAllIds(CancellationToken token = default)
        {
            var result = await _context.Database.SqlQueryRaw<Guid>("SELECT \"Id\" FROM public.\"Tasks\"").ToListAsync(token);

            return result;
        }

        public async Task<Core.Models.Task?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            var taskToFind = await _context.Tasks.FindAsync(id, token);

            return taskToFind;
        }

        public async Task<Guid> GetProjectId(Guid tasksGroupId, CancellationToken cancellationToken = default)
        {
            var project = await _context.Tasks.Where(t => t.TasksGroupId == tasksGroupId).Select(t => t.ProjectId).FirstAsync(cancellationToken);

            return project;
        }

        public async Task<IEnumerable<Core.Models.Task>> GetTasksByGroupId(Guid tasksGroupId, CancellationToken cancellationToken = default)
        {
            var tasks = await _context.Tasks.Where(t => t.TasksGroupId == tasksGroupId).ToListAsync(cancellationToken);

            return tasks;
        }

        public async Task<Core.Models.Task?> UpdateAsync(Core.Models.Task task, CancellationToken token)
        {
            var existingTask = await GetByIdAsync(task.Id, token);

            if (existingTask == null)
            {
                return existingTask;
            }

            _context.Entry(existingTask).CurrentValues.SetValues(task);
            await _context.SaveChangesAsync(token);
            return task;
        }

        public async Task<(IEnumerable<Core.Models.Task>, int)> GetPagedAsync(int page, int pageSize, CancellationToken token)
        {
            var query = _context.Tasks.AsNoTracking();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(token);

            var totalCount = await query.CountAsync(token);

            return (items, totalCount);
        }
    }
}
