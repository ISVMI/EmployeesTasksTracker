
namespace EmployeesTasksTracker.TasksTrackerService.Core.Models
{
    public class TaskEmployee
    {
        public Guid TaskId { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeRoleInTask { get; set; }
    }
}
