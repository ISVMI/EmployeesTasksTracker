using Shared.DTOs;

namespace Shared.Models
{
    public class TasksGroupReportModel
    {
        public string ReportTitle { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.Now;
        public string Name { get; set; }
        public string ProjectName { get; set; }
        public List<TaskForReportDTO> Tasks { get; set; } = new();
    }
}
