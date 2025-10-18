using EmployeesTasksTracker.ProjectsService.Application.Interfaces;
using System.Net.Http.Json;

namespace EmployeesTasksTracker.ProjectsService.Infrastructure.Clients
{
    public class EmployeesClient : IEmployeesClient
    {
        private readonly HttpClient _httpClient;

        public EmployeesClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Guid>?> GetAllEmployeesIds(CancellationToken token = default)
        {
            var responce = await _httpClient.GetAsync("api/employees/getallids", token);

            responce.EnsureSuccessStatusCode();

            return await responce.Content.ReadFromJsonAsync<IEnumerable<Guid>>(token);
        }
    }
}
