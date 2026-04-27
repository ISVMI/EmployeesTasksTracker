using EmployeesTasksTracker.TasksTrackerService.Application.Commands.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.TasksGroups
{
    public class DeleteTasksGroupHandler : IRequestHandler<DeleteTasksGroupCommand, bool>
    {
        private readonly ITasksGroupsRepo _tasksGroupsRepo;

        public DeleteTasksGroupHandler(ITasksGroupsRepo tasksGroupsRepo)
        {
            _tasksGroupsRepo = tasksGroupsRepo;
        }

        public async Task<bool> Handle(DeleteTasksGroupCommand request, CancellationToken cancellationToken)
        {
            var canDelete = await _tasksGroupsRepo.CheckDeletionCapability(request.Id, cancellationToken);

            if (!canDelete)
            {
                throw new DomainException($"Couldn't delete tasks group! Task isn't completed or cancelled!");
            }

            return await _tasksGroupsRepo.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
