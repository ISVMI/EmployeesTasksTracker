namespace EmployeesTasksTracker.EmployeesService.Application.DTOs
{
    public record EditEmployeeDto
    {
        public Guid Id { get; }
        public string Name { get; init; }
        public string Surname { get; init; }
        public string Patronymic { get; init; }
        public string UserName { get; init; }
        public string Role { get; init; }
    }
}
