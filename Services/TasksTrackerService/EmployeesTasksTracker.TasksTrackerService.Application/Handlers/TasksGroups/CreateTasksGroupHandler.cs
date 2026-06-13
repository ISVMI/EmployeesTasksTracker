using EmployeesTasksTracker.TasksTrackerService.Application.Commands.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.TasksGroups
{
    public class CreateTasksGroupHandler : IRequestHandler<CreateTasksGroupCommand, Guid>
    {
        private readonly ITasksGroupsRepo _repo;
        private readonly ILogger<CreateTasksGroupHandler> _logger;

        public CreateTasksGroupHandler(ITasksGroupsRepo repo, ILogger<CreateTasksGroupHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateTasksGroupCommand request, CancellationToken cancellationToken)
        {
            var newTasksGroup = new TasksGroup
            {
                Name = request.TasksGroup.Name
            };

            var result = await _repo.CreateAsync(newTasksGroup, cancellationToken);

            _logger.LogInformation("Successfully created tasks group {tasksGroupId}", result);

            return result;
        }
    }
}
