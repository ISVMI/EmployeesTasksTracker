using EmployeesTasksTracker.EmployeesService.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace EmployeesTasksTracker.EmployeesService.Core.Models
{
    public class Employee
    {
        [Key]
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string UserName { get; set; }
        public Role Role { get; set; }
    }
}
