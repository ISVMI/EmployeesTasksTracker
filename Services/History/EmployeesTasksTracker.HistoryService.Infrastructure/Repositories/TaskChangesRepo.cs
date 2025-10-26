using EmployeesTasksTracker.HistoryService.Core.Interfaces;
using EmployeesTasksTracker.HistoryService.Core.Models;
using EmployeesTasksTracker.HistoryService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeesTasksTracker.HistoryService.Infrastructure.Repositories
{
    public class TaskChangesRepo : ITaskChangesRepo
    {
        private readonly TaskChangesContext _context;

        public TaskChangesRepo(TaskChangesContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateTaskChangesRecord(TaskChanges taskChanges, CancellationToken cancellationToken = default)
        {
            if (taskChanges == null)
            {
                throw new ArgumentNullException(nameof(taskChanges), "Given task changes was null!");
            }

            await _context.TaskChanges.AddAsync(taskChanges, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return taskChanges.Id;
        }

        public async Task<IEnumerable<TaskChanges>> GetAllChanges(CancellationToken cancellationToken = default)
        {
            var changes = await _context.TaskChanges.AsNoTracking().ToListAsync(cancellationToken);

            return changes;
        }

        public async Task<IEnumerable<TaskChanges>> GetChangesByTaskId(Guid taskId, CancellationToken cancellationToken = default)
        {
            var changesByTaskIdQuery = _context.TaskChanges.AsNoTracking().AsQueryable();

            changesByTaskIdQuery = changesByTaskIdQuery.Where(tc => tc.TaskId == taskId);

            var changesByQuery = await changesByTaskIdQuery.ToListAsync(cancellationToken);

            if (!changesByQuery.Any()) 
            {
                throw new Exception($"History for task with id {taskId} was not found");
            }

            return changesByQuery;
        }
    }
}
