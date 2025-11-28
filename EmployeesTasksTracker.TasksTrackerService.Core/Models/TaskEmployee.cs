
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeesTasksTracker.TasksTrackerService.Core.Models
{
    public class TaskEmployee
    {
        public Guid TaskId { get; set; }
        public Task Task { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [Column("EmployeeRoleInTask")]
        public RoleInTask EmployeeRoleInTask { get; set; }
    }
}
