
namespace EmployeesTasksTracker.ProjectsService.Core.Models
{
    public class ProjectsEmployees
    {
        public Guid ProjectId { get; set; }
        public Guid EmployeeId { get; set; }
        public bool IsSupervisor { get; set; }
    }
}
