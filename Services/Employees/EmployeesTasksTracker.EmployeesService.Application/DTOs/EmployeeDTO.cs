namespace EmployeesTasksTracker.EmployeesService.Application.DTOs
{
    public record EmployeeDTO
    {
        public string Name { get; init; }
        public string Surname { get; init; }
        public string Patronymic { get; init; }
        public string UserName { get; init; }
        public string Role { get; init; }
    }
}
