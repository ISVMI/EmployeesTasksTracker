using EmployeesTasksTracker.EmployeesService.Core.Interfaces;
using EmployeesTasksTracker.EmployeesService.Core.Models;
using EmployeesTasksTracker.EmployeesService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeesTasksTracker.EmployeesService.Infrastructure.Repositories
{
    internal class EmployeesRepo : IEmployeesRepo
    {
        private readonly EmployeesContext _context;

        public EmployeesRepo(EmployeesContext context)
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
                throw new Exception("Such employee already exists");
            }

            await _context.Employees.AddAsync(employee, token);
            await _context.SaveChangesAsync(token);
            return employee.Id;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
            try
            {
                var employeeToDelete = await GetByIdAsync(id, token);

                _context.Employees.Remove(employeeToDelete);
                await _context.SaveChangesAsync(token);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not delete employee with the given id {id} : {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<Employee>> GetAllAsync(CancellationToken token = default)
        {
            var query = _context.Employees.AsNoTracking();
            var employees = await query.ToListAsync(token);

            return employees;
        }

        public async Task<IEnumerable<Guid>> GetAllIdsAsync()
        {
            return await _context.Database.
                SqlQueryRaw<Guid>("SELECT \"Id\" FROM public.\"Employees\"")
                .ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            var employeeToFind = await _context.Employees.FindAsync(id, token);

            if (employeeToFind == null)
            {
                throw new Exception($"Employee with id: {id} not found!");
            }

            return employeeToFind;
        }

        public async Task<Employee> UpdateAsync(Employee employee, CancellationToken token = default)
        {
            try
            {
                var existingEmployee = await GetByIdAsync(employee.Id, token);

                _context.Entry(existingEmployee).CurrentValues.SetValues(employee);
                await _context.SaveChangesAsync(token);
                return employee;
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not update employee with username: {employee.UserName}", ex);
            }
        }
    }
}
