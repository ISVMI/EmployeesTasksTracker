using Shared.DTOs;

namespace Shared.Messages
{
    public record TaskCreated
    {
        public Guid TaskId { get; set; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string Deadline { get; init; }
        public string CreatedAt { get; init; }
        public string Status { get; init; }
        public string Priority { get; init; }
    }
}
