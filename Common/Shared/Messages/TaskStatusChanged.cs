using System.Threading.Tasks;

namespace Shared.Messages
{
    public record TaskStatusChanged
    {
        public Guid TaskId { get; set; }
        public string TaskName { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
    }
}
