using System.Threading.Tasks;

namespace Shared.Messages
{
    public record EmployeeAssigned
    {
        public Guid TaskId { get; set; }
        public string TaskName { get; set; }
        public Guid EmployeeId { get; set; }

    }
}
