using System.Text.Json;
using Bogus;
using Bogus.DataSets;
using EmployeesTasksTracker.EmployeesService.Core.Enums;
using EmployeesTasksTracker.EmployeesService.Core.Models;

namespace EmployeesTasksTracker.EmployeesService.Infrastructure.DataSeeding
{
    public static class EmployeesGenerator
    {
        private static readonly Faker _faker = new("ru");

        public static async Task<List<Employee>> GenerateEmployeesAsync(int count)
        {
            var employees = new List<Employee>();
            var employeeGender = _faker.PickRandom(Name.Gender.Male, Name.Gender.Female);
            var patronymics = await GetPatronymics(employeeGender.ToString());

            for (int i = 0; i < count; i++)
            {
                var employee = new Employee
                {
                    Name = _faker.Name.FirstName(employeeGender),
                    Surname = _faker.Name.LastName(employeeGender),
                    Patronymic = _faker.PickRandom(patronymics),
                    Role = _faker.PickRandom<EmployeeRole>(),
                    UserName = _faker.Person.UserName + _faker.Random.AlphaNumeric(4)
                };

                employees.Add(employee);
            }
            return employees;
        }

        private static async Task<string[]> GetPatronymics(string gender)
        {
            if (gender == null)
            {
                throw new ArgumentNullException(nameof(gender), "gender parameter was null!");
            }
            if (gender == "Male")
            {
                await using (FileStream fs = new("DataSeeding\\MalePatronymics.json", FileMode.Open))
                {
                    var malePatronymics = JsonSerializer.Deserialize<string[]>(fs);

                    return malePatronymics;
                }
            }

            await using (FileStream fs = new("DataSeeding\\FemalePatronymics.json", FileMode.Open))
            {
                var femalePatronymics = JsonSerializer.Deserialize<string[]>(fs);
                return femalePatronymics;
            }
        }
    }
}
