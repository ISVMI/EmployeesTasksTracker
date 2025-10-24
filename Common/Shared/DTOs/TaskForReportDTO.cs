namespace Shared.DTOs
{
    public record TaskForReportDTO
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public string Deadline { get; init; }
        public string CreatedAt { get; init; }
        public string Status { get; init; }
        public string Priority { get; init; }
    }
}
