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

        public async Task<Guid> CreateAsync(Project project, CancellationToken token = default)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Given project was null!");
            }

            if (await _context.Projects.AnyAsync(p => p.Name == project.Name, token))
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
            return await _context.Projects.Select(p => p.Id).ToListAsync(token);
        }

        public async Task<Project> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            var projectToFind = await _context.Projects
                .Where(p => p.Id == id)
                .SingleOrDefaultAsync(token);

            if (projectToFind == null)
            {
                throw new DomainException($"Project with id: {id} not found!");
            }

            return projectToFind;
        }

        public async Task<Project> UpdateAsync(Project project, CancellationToken token = default)
        {
            var existingProject = await GetByIdAsync(project.Id, token);

            _context.Entry(existingProject).CurrentValues.SetValues(project);
            await _context.SaveChangesAsync(token);
            return project;
        }
    }
}
