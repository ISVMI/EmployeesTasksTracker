using System.ComponentModel.DataAnnotations;

namespace EmployeesTasksTracker.ProjectsService.Core.Models
{
    public class Project
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid Supervisor { get; set; }
        public Guid Manager { get; set; }

    }
}
