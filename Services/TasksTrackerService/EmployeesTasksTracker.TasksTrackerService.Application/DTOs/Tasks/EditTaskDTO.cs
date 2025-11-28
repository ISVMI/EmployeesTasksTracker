namespace EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Tasks
{
    public record EditTaskDTO
    {
        public Guid Id { get; set; }
        public string Name { get; init; }
        public string Description { get; init; }
        public DateTime Deadline { get; init; }
        public DateTime CreatedAt { get; init; }
        public string Priority { get; init; }
    }
}
