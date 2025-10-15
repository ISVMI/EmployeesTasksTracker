using EmployeesTasksTracker.EmployeesService.Core.Interfaces;
using EmployeesTasksTracker.EmployeesService.Core.Models;
using EmployeesTasksTracker.EmployeesService.Infrastructure.Data;

namespace EmployeesTasksTracker.EmployeesService.Infrastructure.Repositories
{
    internal class EmployeesRepo : IEmployeesRepo
    {
        private readonly EmployeesContext _context;

        public EmployeesRepo(EmployeesContext context)
        {
            _context = context;
        }

        public Task<Guid> CreateAsync(Employee entity, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Employee>> GetAllAsync(CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> UpdateAsync(Employee entity, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
