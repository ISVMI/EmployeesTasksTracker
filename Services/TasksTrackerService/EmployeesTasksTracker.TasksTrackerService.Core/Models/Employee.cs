using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeesTasksTracker.TasksTrackerService.Core.Models
{
    public class Employee
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Column("Name")]
        public string Name { get; set; }
        [Column("Surname")]
        public string Surname { get; set; }
        [Column("Patronymic")]
        public string Patronymic { get; set; }
        [Column("UserName")]
        public string UserName { get; set; }
        [Column("Role")]
        public EmployeeRole Role { get; set; }
        public ICollection<TaskEmployee> TaskEmployees { get; set; } = new HashSet<TaskEmployee>();
        public ICollection<ProjectEmployee> ProjectEmployees { get; set; } = new HashSet<ProjectEmployee>();
    }
}
