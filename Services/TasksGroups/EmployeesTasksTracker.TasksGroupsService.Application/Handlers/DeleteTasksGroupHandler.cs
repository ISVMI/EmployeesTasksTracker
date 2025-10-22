using EmployeesTasksTracker.TasksGroupsService.Application.Commands;
using EmployeesTasksTracker.TasksGroupsService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Handlers
{
    public class DeleteTasksGroupHandler : IRequestHandler<DeleteTasksGroupCommand, bool>
    {
        private readonly ITasksGroupsRepo _repo;

        public DeleteTasksGroupHandler(ITasksGroupsRepo repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteTasksGroupCommand request, CancellationToken cancellationToken)
        {
            return await _repo.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
