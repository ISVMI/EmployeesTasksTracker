using System.Reflection;
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
            var malePatronymics = await GetPatronymics("Male");
            var femalePatronymics = await GetPatronymics("Female");

            if (malePatronymics != null && femalePatronymics != null)
            {
                for (int i = 0; i < count; i++)
                {
                    var employeeGender = _faker.PickRandom(Name.Gender.Male, Name.Gender.Female);
                    var stringGender = employeeGender.ToString();
                    var employee = new Employee
                    {
                        Name = _faker.Name.FirstName(employeeGender),
                        Surname = _faker.Name.LastName(employeeGender),
                        Patronymic = stringGender == "Male" ?
                        _faker.PickRandom(malePatronymics) :
                        _faker.PickRandom(femalePatronymics),
                        Role = _faker.PickRandom<EmployeeRole>(),
                        UserName = _faker.Random.AlphaNumeric(14)
                    };

                    employees.Add(employee);
                }
            }

            return employees;
        }

        private static async Task<string[]?> GetPatronymics(string gender)
        {
            if (gender == null)
            {
                throw new ArgumentNullException(nameof(gender), "gender parameter was null!");
            }

            var assembly = Assembly.GetExecutingAssembly();

            string fileName = "";

            if (gender == "Male")
            {
                fileName = "MalePatronymics.json";
            }
            else
            {
                fileName = "FemalePatronymics.json";
            }

            try
            {
                var json = await GetJsonContentAsync(assembly, fileName);

                var patronymics = JsonSerializer.Deserialize<string[]>(json);

                return patronymics;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        private static string GetResourceName(Assembly assembly, string fileName)
        {
            var resourceNames = assembly.GetManifestResourceNames();
            var resourceName = resourceNames.FirstOrDefault(rn => rn.EndsWith(fileName));
            if (resourceName == null)
            {
                throw new FileNotFoundException($"Resource '{fileName} not found!");
            }

            return resourceName;
        }

        private static async Task<string> GetJsonContentAsync(Assembly assembly, string fileName)
        {
            try
            {
                var resourceName = GetResourceName(assembly, fileName);

                await using var stream = assembly.GetManifestResourceStream(resourceName);

                using var reader = new StreamReader(stream);

                var json = await reader.ReadToEndAsync();

                return json;
            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException($"Could not read json content : {ex.Message}");
            }

        }
    }
}
