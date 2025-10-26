using System.ComponentModel.DataAnnotations;

namespace EmployeesTasksTracker.HistoryService.Core.Models
{
    public class TaskChanges
    {
        [Key]
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public List<string> Changes { get; set; } = new();
        public DateTime ChangedAt { get; set; }
    }
}
