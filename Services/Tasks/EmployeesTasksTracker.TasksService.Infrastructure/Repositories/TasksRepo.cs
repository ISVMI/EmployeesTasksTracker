using EmployeesTasksTracker.TasksService.Core.Interfaces;
using EmployeesTasksTracker.TasksService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
                throw new Exception($"Observer with id {observerId} already exists!");
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
                throw new Exception($"Performer with id {performerId} already exists!");
            }

            taskToEdit.Performers.Add(performerId);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task ChangeStatusAsync(Core.Models.Task task, CancellationToken cancellationToken = default)
        {

            if (task == null)
            {
                throw new ArgumentNullException(nameof(task), "Given task was null!");
            }

            if (task.Status != Core.Enums.Status.Canceled)
            {
                try
                {
                    var existingTask = await GetByIdAsync(task.Id, cancellationToken);

                    if (existingTask.Status == task.Status)
                    {
                        Console.WriteLine("Task status has not changed, because it were the same as before");
                        return;
                    }

                    var exMessage = $"Task can not change from {existingTask.Status} to {task.Status}!";

                    switch (task.Status)
                    {
                        case Core.Enums.Status.Backlog:
                            {
                                if (existingTask.Status != Core.Enums.Status.Backlog)
                                {
                                    throw new ArgumentException(exMessage);
                                }
                                Console.WriteLine("Task status has not changed, because it were and are \"Backlog\"");
                                return;
                            }

                        case Core.Enums.Status.Current:
                            {
                                if (existingTask.Status != Core.Enums.Status.Backlog)
                                {
                                    throw new ArgumentException(exMessage);
                                }
                                break;
                            }

                        case Core.Enums.Status.Active:
                            {
                                if (existingTask.Status != Core.Enums.Status.Current ||
                                    existingTask.Status != Core.Enums.Status.Testing)
                                {
                                    throw new ArgumentException(exMessage);
                                }
                                break;
                            }

                        case Core.Enums.Status.Testing:
                            {
                                if (existingTask.Status != Core.Enums.Status.Active)
                                {
                                    throw new ArgumentException(exMessage);
                                }
                                break;
                            }

                        case Core.Enums.Status.Completed:
                            {
                                if (existingTask.Status != Core.Enums.Status.Testing)
                                {
                                    throw new ArgumentException(exMessage);
                                }
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not change status : {ex.Message}");
                }
            }

            await UpdateAsync(task, cancellationToken);
        }

        public async Task<Guid> CreateAsync(Core.Models.Task task, CancellationToken token = default)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task), "Given task was null!");
            }

            if (task.Status != Core.Enums.Status.Backlog || task.Status != Core.Enums.Status.Current)
            {
                throw new Exception($"Could not create task with the given status - {task.Status}");
            }

            await _context.Tasks.AddAsync(task, token);
            await _context.SaveChangesAsync(token);
            return task.Id;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
            try
            {
                var taskToDelete = await GetByIdAsync(id, token);

                if (taskToDelete.Status == Core.Enums.Status.Active ||
                    taskToDelete.Status == Core.Enums.Status.Testing)
                {
                    Console.WriteLine($"Could not delete task with status - {taskToDelete.Status}");
                    return false;
                }

                _context.Tasks.Remove(taskToDelete);
                await _context.SaveChangesAsync(token);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not delete task with the given id {id} : {ex.Message}");
                return false;
            }
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

        public async Task<Core.Models.Task> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            var taskToFind = await _context.Tasks
                .Where(t => t.Id == id)
                .SingleOrDefaultAsync(token);

            if (taskToFind == null)
            {
                throw new Exception($"Task with id: {id} not found!");
            }

            return taskToFind;
        }

        public async Task<Core.Models.Task> UpdateAsync(Core.Models.Task task, CancellationToken token = default)
        {
            try
            {
                var existingTask = await GetByIdAsync(task.Id, token);

                _context.Entry(existingTask).CurrentValues.SetValues(task);
                await _context.SaveChangesAsync(token);
                return task;
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not update task with id - {task.Id}", ex);
            }
        }
    }
}
