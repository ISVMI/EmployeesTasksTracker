using EmployeesTasksTracker.TasksService.Application.Interfaces;
using System.Net.Http.Json;

namespace EmployeesTasksTracker.TasksService.Infrastructure.Clients
{
    public class TasksGroupsClient : ITasksGroupsClient
    {
        private readonly HttpClient _httpClient;

        public TasksGroupsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Guid>> GetAllIds(CancellationToken cancellationToken = default)
        {
            var responce = await _httpClient.GetAsync($"api/TasksGroups/GetAllTasksGroupsIds", cancellationToken);

            responce.EnsureSuccessStatusCode();

            return await responce.Content.ReadFromJsonAsync<IEnumerable<Guid>>();
        }
    }
}
