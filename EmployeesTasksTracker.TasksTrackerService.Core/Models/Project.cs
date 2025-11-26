using System.ComponentModel.DataAnnotations;

namespace EmployeesTasksTracker.TasksTrackerService.Core.Models
{
    public class Project
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Task> Tasks { get; set; } = new();
    }
}
