namespace EmployeesTasksTracker.TasksTrackerService.Application.DTOs.TasksGroups
{
    public record EditTasksGroupDTO
    {
        public Guid Id { get; set; }
        public string Name { get; init; }
    }
}
