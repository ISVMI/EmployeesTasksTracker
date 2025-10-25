using EmployeesTasksTracker.TasksGroupsService.Application.Interfaces;

namespace EmployeesTasksTracker.TasksGroupsService.Infrastructure.Clients
{
    public class ProjectsClient : IProjectsClient
    {
        private readonly HttpClient _httpClient;

        public ProjectsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetProjectName(Guid id, CancellationToken cancellationToken = default)
        {
            var responce = await _httpClient.GetAsync($"api/Projects/{id}?nameRequested=true", cancellationToken);

            responce.EnsureSuccessStatusCode();

            return await responce.Content.ReadAsStringAsync();
        }
    }
}
