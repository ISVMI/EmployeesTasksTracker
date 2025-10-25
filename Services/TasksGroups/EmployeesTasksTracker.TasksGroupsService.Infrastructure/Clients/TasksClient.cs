using EmployeesTasksTracker.TasksGroupsService.Application.Interfaces;
using Shared.DTOs;
using System.Net.Http.Json;

namespace EmployeesTasksTracker.TasksGroupsService.Infrastructure.Clients
{
    public class TasksClient : ITasksClient
    {
        private readonly HttpClient _httpClient;

        public TasksClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Guid> GetProjectId(Guid tasksGroupId, CancellationToken cancellationToken = default)
        {
            var responce = await _httpClient.GetAsync($"api/Tasks/GetProjectId/{tasksGroupId}", cancellationToken);

            responce.EnsureSuccessStatusCode();

            return await responce.Content.ReadFromJsonAsync<Guid>();
        }

        public async Task<IEnumerable<TaskForReportDTO>> GetTasks(Guid tasksGroupId, CancellationToken cancellationToken = default)
        {
            var responce = await _httpClient.GetAsync($"api/Tasks/GetTasksByGroupId/{tasksGroupId}", cancellationToken);

            responce.EnsureSuccessStatusCode();

            return await responce.Content.ReadFromJsonAsync<IEnumerable<TaskForReportDTO>>();
        }
    }
}
