using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeesTasksTracker.TasksTrackerService.Core.Models
{
    public class Project
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Column("Name")]
        public string Name { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        public ICollection<Task> Tasks { get; set; } = new HashSet<Task>();
        public ICollection<ProjectEmployee> ProjectEmployees { get; set; } = new HashSet<ProjectEmployee>();
    }
}
