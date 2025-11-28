using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using EmployeesTasksTracker.TasksTrackerService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Infrastructure.Repositories
{
    internal class EmployeesRepo : IEmployeesRepo
    {
        private readonly TasksTrackerContext _context;

        public EmployeesRepo(TasksTrackerContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(Employee employee, CancellationToken token = default)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), "Given employee was null!");
            }

            if (await _context.Employees.AnyAsync(e => e.UserName  == employee.UserName, token))
            {
                throw new DomainException("Such employee already exists");
            }

            await _context.Employees.AddAsync(employee, token);
            await _context.SaveChangesAsync(token);
            return employee.Id;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
                var employeeToDelete = await GetByIdAsync(id, token);

                _context.Employees.Remove(employeeToDelete);
                await _context.SaveChangesAsync(token);
                return true;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync(CancellationToken token = default)
        {
            var query = _context.Employees.AsNoTracking();
            var employees = await query.ToListAsync(token);

            return employees;
        }

        public async Task<IEnumerable<Guid>> GetAllIds(CancellationToken token = default)
        {
            return await _context.Employees.Select(e => e.Id).ToListAsync(token);
        }

        public async Task<Employee> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            var employeeToFind = await _context.Employees.FindAsync(id, token);

            if (employeeToFind == null)
            {
                throw new DomainException($"Employee with id: {id} not found!");
            }

            return employeeToFind;
        }

        public async Task<Employee> UpdateAsync(Employee employee, CancellationToken token = default)
        {
                var existingEmployee = await GetByIdAsync(employee.Id, token);

                _context.Entry(existingEmployee).CurrentValues.SetValues(employee);
                await _context.SaveChangesAsync(token);
                return employee;
        }
    }
}
