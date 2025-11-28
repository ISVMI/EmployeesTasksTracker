using EmployeesTasksTracker.TasksTrackerService.Application.Commands.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.TasksGroups
{
    public class CreateTasksGroupHandler : IRequestHandler<CreateTasksGroupCommand, Guid>
    {
        private readonly ITasksGroupsRepo _repo;

        public CreateTasksGroupHandler(ITasksGroupsRepo repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(CreateTasksGroupCommand request, CancellationToken cancellationToken)
        {
            var newTasksGroup = new TasksGroup
            {
                Name = request.TasksGroup.Name
            };

            await _repo.CreateAsync(newTasksGroup, cancellationToken);

            return newTasksGroup.Id;
        }
    }
}
