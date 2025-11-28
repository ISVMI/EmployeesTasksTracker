namespace EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Tasks
{
    public record CreateTaskDTO
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public DateTime Deadline { get; init; }
        public DateTime CreatedAt { get; init; } = DateTime.Now;
        public string Status { get; init; }
        public string Priority { get; init; }
    }
}
