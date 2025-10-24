namespace Shared.DTOs
{
    public record EmployeeForReportDTO
    {
        public string Name { get; init; }
        public string Surname { get; init; }
        public string Patronymic { get; init; }
        public string Role { get; init; }
    }
}
