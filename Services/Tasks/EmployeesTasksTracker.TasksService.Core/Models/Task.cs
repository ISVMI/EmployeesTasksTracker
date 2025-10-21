using EmployeesTasksTracker.TasksService.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace EmployeesTasksTracker.TasksService.Core.Models
{
    public class Task
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid Project { get; init; }
        public Guid TasksGroup { get; init; }
        public DateTime Deadline { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public List<Guid> Performers { get; set; }
        public List<Guid> Observers { get; set; }
    }
}
