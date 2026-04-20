using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using EmployeesTasksTracker.TasksTrackerService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Infrastructure.Repositories
{
    public class TasksGroupsRepo : ITasksGroupsRepo
    {
        private readonly TasksTrackerContext _context;

        public TasksGroupsRepo(TasksTrackerContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(TasksGroup tasksGroup, CancellationToken token = default)
        {
            if (tasksGroup == null)
            {
                throw new ArgumentNullException(nameof(tasksGroup), "Given tasks group was null!");
            }

            if (await _context.TasksGroups.AnyAsync(tg => tg.Name == tasksGroup.Name))
            {
                throw new DomainException("Such tasks group already exists");
            }

            await _context.TasksGroups.AddAsync(tasksGroup, token);
            await _context.SaveChangesAsync(token);
            return tasksGroup.Id;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
            var taskGroupToDelete = await GetByIdAsync(id, token);

            _context.TasksGroups.Remove(taskGroupToDelete);
            await _context.SaveChangesAsync(token);
            return true;
        }

        public async Task<IEnumerable<TasksGroup>> GetAllAsync(CancellationToken token = default)
        {
            var query = _context.TasksGroups.AsNoTracking();
            var tasksGroups = await query.ToListAsync(token);

            return tasksGroups;
        }

        public async Task<IEnumerable<Guid>> GetAllIds(CancellationToken token = default)
        {
            var result = await _context.Database.SqlQueryRaw<Guid>("SELECT \"Id\" FROM public.\"TasksGroups\"").ToListAsync(token);

            return result;
        }

        public async Task<TasksGroup> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            var taskGroupToFind = await _context.TasksGroups.FindAsync(id, token);

            if (taskGroupToFind == null)
            {
                throw new DomainException($"Tasks group with id: {id} not found!");
            }

            return taskGroupToFind;
        }

        public async Task<(IEnumerable<TasksGroup>, int)> GetPagedAsync(int page, int pageSize, CancellationToken token = default)
        {
            var query = _context.TasksGroups.AsNoTracking();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(token);

            var totalCount = await query.CountAsync(token);

            return (items, totalCount);
        }

        public async Task<TasksGroup> UpdateAsync(TasksGroup taskGroup, CancellationToken token = default)
        {
            var existingTaskGroup = await GetByIdAsync(taskGroup.Id, token);

            _context.Entry(existingTaskGroup).CurrentValues.SetValues(taskGroup);
            await _context.SaveChangesAsync(token);
            return taskGroup;
        }
    }
}
