
namespace EmployeesTasksTracker.TasksTrackerService.Core.Models
{
    public class ProjectEmployee
    {
        public Guid ProjectId { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeRoleInProject { get; set; }
    }
}
