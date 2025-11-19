using EmployeesTasksTracker.TasksService.Core.Interfaces;
using EmployeesTasksTracker.TasksService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksService.Infrastructure.Repositories
{
    public class TasksRepo : ITasksRepo
    {
        private readonly TasksContext _context;

        public TasksRepo(TasksContext context)
        {
            _context = context;
        }

        public async Task AddObserverAsync(Guid observerId, Guid taskId, CancellationToken cancellationToken = default)
        {
            if (observerId == Guid.Empty)
            {
                throw new ArgumentException($"Given observer's id was empty - {observerId}", nameof(observerId));
            }

            var taskToEdit = await GetByIdAsync(taskId, cancellationToken);

            if (taskToEdit == null)
            {
                throw new ArgumentNullException(nameof(taskToEdit), $"Could not find task with the given id - {taskId}");
            }

            if (taskToEdit.Observers.Contains(observerId))
            {
                throw new DomainException($"Observer with id {observerId} already exists!");
            }

            if (taskToEdit.Performers.Contains(observerId))
            {
                throw new DomainException($"Employee with id {observerId} already assigned as performer!");
            }

            taskToEdit.Observers.Add(observerId);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddPerformerAsync(Guid performerId, Guid taskId, CancellationToken cancellationToken = default)
        {
            if (performerId == Guid.Empty)
            {
                throw new ArgumentException($"Given performer's id was empty - {performerId}", nameof(performerId));
            }

            var taskToEdit = await GetByIdAsync(taskId, cancellationToken);

            if (taskToEdit == null)
            {
                throw new ArgumentNullException(nameof(taskToEdit), $"Could not find task with the given id - {taskId}");
            }

            if (taskToEdit.Performers.Contains(performerId))
            {
                throw new DomainException($"Performer with id {performerId} already exists!");
            }

            if (taskToEdit.Observers.Contains(performerId))
            {
                throw new DomainException($"Employee with id {performerId} already assigned as observer!");
            }

            taskToEdit.Performers.Add(performerId);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Guid> CreateAsync(Core.Models.Task task, CancellationToken token = default)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task), "Given task was null!");
            }

            if (task.Status != Core.Enums.Status.Backlog && task.Status != Core.Enums.Status.Current)
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

            if (taskToDelete.Status == Core.Enums.Status.Active ||
                taskToDelete.Status == Core.Enums.Status.Testing)
            {
                throw new DomainException($"Could not delete task with status - {taskToDelete.Status}");
            }

            _context.Tasks.Remove(taskToDelete);
            await _context.SaveChangesAsync(token);
            return true;
        }

        public async Task<IEnumerable<Core.Models.Task>> GetAllAsync(
            Guid? employeeId = null,
            Guid? tasksGroupId = null,
            Guid? projectId = null,
            CancellationToken token = default)
        {
            var query = _context.Tasks.AsNoTracking().AsQueryable();

            if (employeeId.HasValue && employeeId != Guid.Empty)
            {
                query = query.Where(t => t.Performers.Contains(employeeId.Value) || t.Observers.Contains(employeeId.Value));
            }

            if (tasksGroupId.HasValue && tasksGroupId != Guid.Empty)
            {
                query = query.Where(t => t.TasksGroup == tasksGroupId.Value);
            }

            if (projectId.HasValue && projectId != Guid.Empty)
            {
                query = query.Where(t => t.Project == projectId.Value);
            }

            var tasks = await query.ToListAsync(token);

            return tasks;
        }

        public async Task<IEnumerable<Guid>> GetAllIds(CancellationToken token = default)
        {
            return await _context.Database.
                SqlQueryRaw<Guid>("SELECT \"Id\" FROM public.\"Tasks\"")
                .ToListAsync();
        }

        public async Task<Core.Models.Task> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            var taskToFind = await _context.Tasks
                .Where(t => t.Id == id)
                .SingleOrDefaultAsync(token);

            if (taskToFind == null)
            {
                throw new DomainException($"Task with id: {id} not found!");
            }

            return taskToFind;
        }

        public async Task<Guid> GetProjectId(Guid tasksGroupId, CancellationToken cancellationToken = default)
        {
            var project = await _context.Tasks.Where(t => t.TasksGroup == tasksGroupId).Select(t => t.Project).FirstAsync(cancellationToken);

            return project;
        }

        public async Task<IEnumerable<Core.Models.Task>> GetTasksByGroupId(Guid tasksGroupId, CancellationToken cancellationToken = default)
        {
            var tasks = await _context.Tasks.Where(t => t.TasksGroup == tasksGroupId).ToListAsync(cancellationToken);

            return tasks;
        }

        public async Task<Core.Models.Task> UpdateAsync(Core.Models.Task task, CancellationToken token = default)
        {
                var existingTask = await GetByIdAsync(task.Id, token);

                _context.Entry(existingTask).CurrentValues.SetValues(task);
                await _context.SaveChangesAsync(token);
                return task;
        }
    }
}
