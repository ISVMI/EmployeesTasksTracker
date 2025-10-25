using Shared.DTOs;

namespace Shared.Models
{
    public class TaskReportModel
    {
        public string ReportTitle { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.Now;
        public string TaskName { get; set; }
        public string Description { get; set; }
        public string Deadline { get; set; }
        public string CreatedAt { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string ProjectName { get; set; }
        public string TaskGroupName { get; set; }
        public List<EmployeeForReportDTO> Performers { get; set; } = new();
        public List<EmployeeForReportDTO> Observers { get; set; } = new();
    }
}
