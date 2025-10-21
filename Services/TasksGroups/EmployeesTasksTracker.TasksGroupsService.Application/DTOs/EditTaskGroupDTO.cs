namespace EmployeesTasksTracker.TasksGroupsService.Application.DTOs
{
    public record EditTaskGroupDTO
    {
        public Guid Id { get; set; }
        public string Name { get; init; }
    }
}
