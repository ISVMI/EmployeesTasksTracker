namespace EmployeesTasksTracker.HistoryService.Application.DTOs
{
    public record TaskChangesDTO
    {
        public Guid TaskId { get; init; }
        public List<string> Changes { get; init; }
        public string ChangedAt { get; init; }
    }
}
