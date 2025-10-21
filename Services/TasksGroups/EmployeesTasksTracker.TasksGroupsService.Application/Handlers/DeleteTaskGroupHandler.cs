using EmployeesTasksTracker.TasksGroupsService.Application.Commands;
using EmployeesTasksTracker.TasksGroupsService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Handlers
{
    public class DeleteTaskGroupHandler : IRequestHandler<DeleteTaskGroupCommand, bool>
    {
        private readonly ITaskGroupsRepo _repo;

        public DeleteTaskGroupHandler(ITaskGroupsRepo repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteTaskGroupCommand request, CancellationToken cancellationToken)
        {
            return await _repo.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
