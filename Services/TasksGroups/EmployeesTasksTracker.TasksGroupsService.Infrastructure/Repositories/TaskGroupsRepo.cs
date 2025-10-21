using EmployeesTasksTracker.TasksGroupsService.Core.Interfaces;
using EmployeesTasksTracker.TasksGroupsService.Core.Models;
using EmployeesTasksTracker.TasksGroupsService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeesTasksTracker.TasksGroupsService.Infrastructure.Repositories
{
    public class TaskGroupsRepo : ITaskGroupsRepo
    {
        private readonly TaskGroupsContext _context;

        public TaskGroupsRepo(TaskGroupsContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(TaskGroup taskGroup, CancellationToken token = default)
        {
            if (taskGroup == null)
            {
                throw new ArgumentNullException(nameof(taskGroup), "Given project was null!");
            }

            if (await _context.TaskGroups.AnyAsync(tg => tg.Name == taskGroup.Name))
            {
                throw new Exception("Such task group already exists");
            }

            await _context.TaskGroups.AddAsync(taskGroup, token);
            await _context.SaveChangesAsync(token);
            return taskGroup.Id;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
            try
            {
                var taskGroupToDelete = await GetByIdAsync(id, token);

                _context.TaskGroups.Remove(taskGroupToDelete);
                await _context.SaveChangesAsync(token);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not delete task group with the given id {id} : {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<TaskGroup>> GetAllAsync(CancellationToken token = default)
        {
            var query = _context.TaskGroups.AsNoTracking();
            var taskGroups = await query.ToListAsync(token);

            return taskGroups;
        }

        public async Task<TaskGroup> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            var taskGroupToFind = await _context.TaskGroups.FindAsync(id, token);

            if (taskGroupToFind == null)
            {
                throw new Exception($"Task group with id: {id} not found!");
            }

            return taskGroupToFind;
        }

        public async Task<TaskGroup> UpdateAsync(TaskGroup taskGroup, CancellationToken token = default)
        {
            try
            {
                var existingTaskGroup = await GetByIdAsync(taskGroup.Id, token);

                _context.Entry(existingTaskGroup).CurrentValues.SetValues(taskGroup);
                await _context.SaveChangesAsync(token);
                return taskGroup;
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not update task group named: {taskGroup.Name}", ex);
            }
        }
    }
}
