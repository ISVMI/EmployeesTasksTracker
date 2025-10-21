using System.ComponentModel.DataAnnotations;

namespace EmployeesTasksTracker.TasksGroupsService.Core.Models
{
    public class TasksGroup
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
    }
}
