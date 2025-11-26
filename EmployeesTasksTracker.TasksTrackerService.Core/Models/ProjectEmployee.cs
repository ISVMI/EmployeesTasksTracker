
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeesTasksTracker.TasksTrackerService.Core.Models
{
    public class ProjectEmployee
    {
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [Column("EmployeeRoleInProject")]
        public string EmployeeRoleInProject { get; set; }
    }
}
