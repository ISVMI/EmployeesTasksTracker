namespace EmployeesTasksTracker.TasksService.Application.DTOs
{
    public record TaskDTO
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public Guid Project { get; init; }
        public Guid TasksGroup { get; init; }
        public DateTime Deadline { get; init; }
        public DateTime CreatedAt { get; init; }
        public string Status { get; init; }
        public string Priority { get; init; }
        public List<Guid> Performers { get; init; }
        public List<Guid> Observers { get; init; }
    }
}
