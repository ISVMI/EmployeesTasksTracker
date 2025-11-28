using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using Shared.Interfaces;

namespace EmployeesTasksTracker.TasksTrackerService.Core.Interfaces
{
    public interface IEmployeesRepo : IRepository<Employee>, IIdsGetter
    {
    }
}
