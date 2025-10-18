using EmployeesTasksTracker.EmployeesService.Core.Models;
using Shared.Interfaces;

namespace EmployeesTasksTracker.EmployeesService.Core.Interfaces
{
    public interface IEmployeesRepo : IRepository<Employee>
    {
        Task<IEnumerable<Guid>> GetAllIdsAsync();
    }
}
