using EmployeesTasksTracker.TasksService.Application.Interfaces;
using System.Net.Http.Json;

namespace EmployeesTasksTracker.TasksService.Infrastructure.Clients
{
    public class ProjectsClient : IProjectsClient
    {
        private readonly HttpClient _httpClient;

        public ProjectsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Guid>> GetAllIds(CancellationToken cancellationToken = default)
        {
            var responce = await _httpClient.GetAsync($"api/Projects/GetAllProjectsIds", cancellationToken);

            responce.EnsureSuccessStatusCode();

            return await responce.Content.ReadFromJsonAsync<IEnumerable<Guid>>();
        }

        public async Task<string> GetProjectName(Guid id, CancellationToken cancellationToken = default)
        {
            var responce = await _httpClient.GetAsync($"api/Projects/{id}?nameRequested=true", cancellationToken);

            responce.EnsureSuccessStatusCode();

            return await responce.Content.ReadFromJsonAsync<string>();
        }
    }
}
