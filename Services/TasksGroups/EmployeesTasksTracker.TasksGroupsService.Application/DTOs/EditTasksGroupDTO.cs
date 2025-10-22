namespace EmployeesTasksTracker.TasksGroupsService.Application.DTOs
{
    public record EditTasksGroupDTO
    {
        public Guid Id { get; set; }
        public string Name { get; init; }
    }
}
