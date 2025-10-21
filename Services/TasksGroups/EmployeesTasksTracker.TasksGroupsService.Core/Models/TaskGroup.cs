using System.ComponentModel.DataAnnotations;

namespace EmployeesTasksTracker.TasksGroupsService.Core.Models
{
    public class TaskGroup
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
    }
}
