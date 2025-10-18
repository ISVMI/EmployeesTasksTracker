namespace EmployeesTasksTracker.ProjectsService.Application.DTOs
{
    public record ProjectDTO
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public Guid Supervisor { get; init; }
        public Guid Manager { get; init; }
    }
}
