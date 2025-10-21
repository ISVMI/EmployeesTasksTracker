using EmployeesTasksTracker.ProjectsService.Core.Interfaces;
using EmployeesTasksTracker.ProjectsService.Core.Models;
using EmployeesTasksTracker.ProjectsService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeesTasksTracker.ProjectsService.Infrastructure.Repositories
{
    public class ProjectsRepo : IProjectsRepo
    {
        private readonly ProjectsContext _context;

        public ProjectsRepo(ProjectsContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(Project project, CancellationToken token = default)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Given project was null!");
            }

            if (await _context.Projects.AnyAsync(e => e.Name == project.Name, token))
            {
                throw new Exception("Such project already exists");
            }

            await _context.Projects.AddAsync(project, token);
            await _context.SaveChangesAsync(token);
            return project.Id;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
            try
            {
                var employeeToDelete = await GetByIdAsync(id, token);

                _context.Projects.Remove(employeeToDelete);
                await _context.SaveChangesAsync(token);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not delete project with the given id {id} : {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<Project>> GetAllAsync(CancellationToken token = default)
        {
            var query = _context.Projects.AsNoTracking();
            var projects = await query.ToListAsync(token);

            return projects;
        }

        public async Task<Project> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            var employeeToFind = await _context.Projects.FindAsync(id, token);

            if (employeeToFind == null)
            {
                throw new Exception($"Project with id: {id} not found!");
            }

            return employeeToFind;
        }

        public async Task<Project> UpdateAsync(Project project, CancellationToken token = default)
        {
            try
            {
                var existingEmployee = await GetByIdAsync(project.Id, token);

                _context.Entry(existingEmployee).CurrentValues.SetValues(project);
                await _context.SaveChangesAsync(token);
                return project;
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not update project named: {project.Name}", ex);
            }
        }
    }
}
