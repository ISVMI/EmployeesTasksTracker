using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using EmployeesTasksTracker.TasksTrackerService.Infrastructure.Data;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Infrastructure.Repositories
{
    public class ProjectEmployeeRepo : IProjectEmployeeRepo
    {
        private readonly TasksTrackerContext _context;

        public ProjectEmployeeRepo(TasksTrackerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProjectEmployee>> GetAllById(Guid? projectId = null, Guid? employeeId = null, CancellationToken token = default)
        {
            if (projectId == null && employeeId == null)
            {
                throw new DomainException($"Both parameters were null");
            }

            if (employeeId == null) 
            {
                return _context.ProjectEmployees.Where(pe => pe.ProjectId == projectId).ToList();
            }

            if (projectId == null)
            {
                return _context.ProjectEmployees.Where(pe => pe.EmployeeId == employeeId).ToList();
            }

            return _context.ProjectEmployees.Where(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId).ToList();
        }
    }
}
