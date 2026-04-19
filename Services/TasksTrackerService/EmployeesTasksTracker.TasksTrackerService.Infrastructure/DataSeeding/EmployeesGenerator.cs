using Bogus;
using Bogus.DataSets;
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using Npgsql;
using Shared.Methods;
using System.Reflection;
using System.Text.Json;

namespace EmployeesTasksTracker.TasksTrackerService.Infrastructure.DataSeeding
{
    public static class EmployeesGenerator
    {
        private static readonly Faker _faker = new("ru");

        public static async System.Threading.Tasks.Task GenerateEmployees(int count, int batchSize, string connectionString)
        {
            var employees = BatchesGenerator.GenerateBatches<Employee>(count, batchSize, EmployeesGenerator.GenerateEmployee);

            await Parallel.ForEachAsync(employees, new ParallelOptions {MaxDegreeOfParallelism = 4 }, async (batch, _) =>
            {
                await using var connection = new NpgsqlConnection(connectionString);

                await connection.OpenAsync();

                await using var writer = connection.BeginBinaryImport(@" COPY ""Employees"" 
                                    (""Id"", ""Name"", ""Surname"", ""Patronymic"", ""Role"", ""UserName"")
                                    FROM STDIN (FORMAT BINARY)");

                foreach (var employee in batch)
                {
                    writer.StartRow();
                    writer.Write(employee.Id);
                    writer.Write(employee.Name);
                    writer.Write(employee.Surname);
                    writer.Write(employee.Patronymic);
                    writer.Write((int)employee.Role);
                    writer.Write(employee.UserName);
                }

                await writer.CompleteAsync();
            });
        }

        private static Employee GenerateEmployee()
        {
            var malePatronymics = GetPatronymics("Male");
            var femalePatronymics = GetPatronymics("Female");

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

            return employee;
        }

        private static string[]? GetPatronymics(string gender)
        {
            if (gender == null)
            {
                throw new ArgumentNullException(nameof(gender), "gender parameter was null!");
            }

            var assembly = Assembly.GetExecutingAssembly();

            string fileName;

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
                var json = GetJsonContent(assembly, fileName);

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

        private static string GetJsonContent(Assembly assembly, string fileName)
        {
            try
            {
                var resourceName = GetResourceName(assembly, fileName);

                using var stream = assembly.GetManifestResourceStream(resourceName);

                using var reader = new StreamReader(stream);

                var json = reader.ReadToEnd();

                return json;
            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException($"Could not read json content : {ex.Message}");
            }

        }
    }
}
