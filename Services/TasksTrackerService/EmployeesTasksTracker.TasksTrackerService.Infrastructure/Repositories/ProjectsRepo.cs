using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using EmployeesTasksTracker.TasksTrackerService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Infrastructure.Repositories
{
    public class ProjectsRepo : IProjectsRepo
    {
        private readonly TasksTrackerContext _context;

        public ProjectsRepo(TasksTrackerContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckDeletionCapability(Guid projectId, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks.AnyAsync(t => t.ProjectId == projectId &&
            (t.Status == Core.Enums.Status.Canceled || t.Status == Core.Enums.Status.Completed), cancellationToken);
        }

        public async Task<Guid> CreateAsync(Project project, CancellationToken token = default)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Given project was null!");
            }

            if (await _context.Projects.AnyAsync(p => p.Name == project.Name && p.Description == project.Description, token))
            {
                throw new DomainException("Such project already exists");
            }

            await _context.Projects.AddAsync(project, token);
            await _context.SaveChangesAsync(token);
            return project.Id;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
            var projectToDelete = await GetByIdAsync(id, token);

            if(projectToDelete is null)
            {
                throw new NotFoundException("project", id);
            }

            _context.Projects.Remove(projectToDelete);
            await _context.SaveChangesAsync(token);
            return true;
        }

        public async Task<IEnumerable<Project>> GetAllAsync(CancellationToken token = default)
        {
            var query = _context.Projects.AsNoTracking();
            var projects = await query.ToListAsync(token);

            return projects;
        }

        public async Task<IEnumerable<Guid>> GetAllIds(CancellationToken token = default)
        {
            var result = await _context.Database.SqlQueryRaw<Guid>("SELECT \"Id\" FROM public.\"Projects\"").ToListAsync(token);

            return result;
        }

        public async Task<Project> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            var projectToFind = await _context.Projects.FindAsync(id, token);

            return projectToFind;
        }

        public async Task<(IEnumerable<Project>, int)> GetPagedAsync(int page, int pageSize, CancellationToken token = default)
        {
            var query = _context.Projects.AsNoTracking();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(token);

            var totalCount = await query.CountAsync(token);

            return (items, totalCount);
        }

        public async Task<Project> UpdateAsync(Project project, CancellationToken token = default)
        {
            var existingProject = await GetByIdAsync(project.Id, token);

            if (existingProject is null) 
            {
                return existingProject;
            }

            _context.Entry(existingProject).CurrentValues.SetValues(project);
            await _context.SaveChangesAsync(token);
            return project;
        }
    }
}
