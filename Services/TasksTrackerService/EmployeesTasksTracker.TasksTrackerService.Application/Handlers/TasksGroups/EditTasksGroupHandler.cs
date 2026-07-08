using EmployeesTasksTracker.TasksTrackerService.Application.Commands.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.TasksGroups
{
    public class EditTasksGroupHandler : IRequestHandler<EditTasksGroupCommand, TasksGroupDTO>
    {
        private readonly ITasksGroupsRepo _repo;
        private readonly HybridCache _cache;
        private readonly ILogger<EditTasksGroupHandler> _logger;

        public EditTasksGroupHandler(ITasksGroupsRepo repo, HybridCache cache, ILogger<EditTasksGroupHandler> logger)
        {
            _repo = repo;
            _cache = cache;
            _logger = logger;
        }

        public async Task<TasksGroupDTO> Handle(EditTasksGroupCommand request, CancellationToken cancellationToken)
        {
            var tasksGroupToEdit = new TasksGroup
            {
                Id = request.TasksGroupToEdit.Id,
                Name = request.TasksGroupToEdit.Name
            };

            await _repo.UpdateAsync(tasksGroupToEdit, cancellationToken);

            await _cache.RemoveAsync($"tasksgroup:{request.TasksGroupToEdit.Id}", cancellationToken);

            _logger.LogInformation("Successfully edited tasks group {tasksGroupId}", request.TasksGroupToEdit.Id);

            return new TasksGroupDTO
            {
                Name = tasksGroupToEdit.Name
            };
        }
    }
}
