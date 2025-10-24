using EmployeesTasksTracker.TasksService.Application.Interfaces;
using Shared.DTOs;
using System.Net.Http.Json;

namespace EmployeesTasksTracker.TasksService.Infrastructure.Clients
{
    public class EmployeesClient : IEmployeesClient
    {
        private readonly HttpClient _httpClient;

        public EmployeesClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Guid>> GetAllIds(CancellationToken cancellationToken = default)
        {
            var responce = await _httpClient.GetAsync($"api/Employees/GetAllEmployeesIds");

            responce.EnsureSuccessStatusCode();

            return await responce.Content.ReadFromJsonAsync<IEnumerable<Guid>>();
        }

        public async Task<IEnumerable<EmployeeForReportDTO>> GetEmployeesInfo(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {

            var responce = await _httpClient.PostAsJsonAsync($"api/Employees/GetEmployeesInfo", ids, cancellationToken);

            responce.EnsureSuccessStatusCode();

            return await responce.Content.ReadFromJsonAsync<IEnumerable<EmployeeForReportDTO>>();

        }
    }
}
