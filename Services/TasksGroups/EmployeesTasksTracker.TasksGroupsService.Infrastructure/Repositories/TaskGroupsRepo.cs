using EmployeesTasksTracker.TasksGroupsService.Core.Interfaces;
using EmployeesTasksTracker.TasksGroupsService.Core.Models;
using EmployeesTasksTracker.TasksGroupsService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeesTasksTracker.TasksGroupsService.Infrastructure.Repositories
{
    public class TaskGroupsRepo : ITaskGroupsRepo
    {
        private readonly TasksGroupsContext _context;

        public TaskGroupsRepo(TasksGroupsContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(TasksGroup tasksGroup, CancellationToken token = default)
        {
            if (tasksGroup == null)
            {
                throw new ArgumentNullException(nameof(tasksGroup), "Given project was null!");
            }

            if (await _context.TasksGroups.AnyAsync(tg => tg.Name == tasksGroup.Name))
            {
                throw new Exception("Such task group already exists");
            }

            await _context.TasksGroups.AddAsync(tasksGroup, token);
            await _context.SaveChangesAsync(token);
            return tasksGroup.Id;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
            try
            {
                var taskGroupToDelete = await GetByIdAsync(id, token);

                _context.TasksGroups.Remove(taskGroupToDelete);
                await _context.SaveChangesAsync(token);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not delete task group with the given id {id} : {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<TasksGroup>> GetAllAsync(CancellationToken token = default)
        {
            var query = _context.TasksGroups.AsNoTracking();
            var tasksGroups = await query.ToListAsync(token);

            return tasksGroups;
        }

        public async Task<TasksGroup> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            var taskGroupToFind = await _context.TasksGroups.FindAsync(id, token);

            if (taskGroupToFind == null)
            {
                throw new Exception($"Task group with id: {id} not found!");
            }

            return taskGroupToFind;
        }

        public async Task<TasksGroup> UpdateAsync(TasksGroup taskGroup, CancellationToken token = default)
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
