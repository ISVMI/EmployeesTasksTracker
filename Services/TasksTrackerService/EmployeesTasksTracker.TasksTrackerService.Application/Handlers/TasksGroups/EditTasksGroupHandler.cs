using EmployeesTasksTracker.TasksTrackerService.Application.Commands.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.TasksGroups
{
    public class EditTasksGroupHandler : IRequestHandler<EditTasksGroupCommand, TasksGroupDTO>
    {
        private readonly ITasksGroupsRepo _repo;
        private readonly IDistributedCache _cache;

        public EditTasksGroupHandler(ITasksGroupsRepo repo, IDistributedCache cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public async Task<TasksGroupDTO> Handle(EditTasksGroupCommand request, CancellationToken cancellationToken)
        {
            var tasksGroupToEdit = new TasksGroup
            {
                Id = request.TasksGroupToEdit.Id,
                Name = request.TasksGroupToEdit.Name
            };

            await _repo.UpdateAsync(tasksGroupToEdit,cancellationToken);

            await _cache.RemoveAsync($"tasksgroup:{request.TasksGroupToEdit.Id}", cancellationToken);

            return new TasksGroupDTO
            {
                Name = tasksGroupToEdit.Name
            };
        }
    }
}
