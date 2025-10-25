using Shared.DTOs;

namespace Shared.Models
{
    public class TaskReportData
    {
        public TaskForReportDTO Task {  get; set; }
        public string ProjectName { get; set; }
        public string TasksGroupName { get; set; }
        public List<EmployeeForReportDTO> Performers { get; set; }
        public List<EmployeeForReportDTO> Observers { get; set; }
    }
}
