using EmployeesTasksTracker.TasksTrackerService.Application.Commands.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.TasksGroups
{
    public class EditTasksGroupHandler : IRequestHandler<EditTasksGroupCommand, TasksGroupDTO>
    {
        private readonly ITasksGroupsRepo _repo;

        public EditTasksGroupHandler(ITasksGroupsRepo repo)
        {
            _repo = repo;
        }

        public async Task<TasksGroupDTO> Handle(EditTasksGroupCommand request, CancellationToken cancellationToken)
        {
            var tasksGroupToEdit = new TasksGroup
            {
                Id = request.TasksGroupToEdit.Id,
                Name = request.TasksGroupToEdit.Name
            };

            await _repo.UpdateAsync(tasksGroupToEdit,cancellationToken);

            return new TasksGroupDTO
            {
                Name = tasksGroupToEdit.Name
            };
        }
    }
}
