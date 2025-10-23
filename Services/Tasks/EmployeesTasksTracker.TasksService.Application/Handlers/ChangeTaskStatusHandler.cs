using EmployeesTasksTracker.TasksService.Application.Commands;
using EmployeesTasksTracker.TasksService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksService.Application.Handlers
{
    public class ChangeTaskStatusHandler : IRequestHandler<ChangeTaskStatusCommand>
    {
        private readonly ITasksRepo _repo;

        public ChangeTaskStatusHandler(ITasksRepo repo)
        {
            _repo = repo;
        }

        public async Task Handle(ChangeTaskStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _repo.ChangeStatusAsync(request.TaskId, request.NewStatus, cancellationToken);
            }
            catch (Exception ex) 
            {
                throw new Exception($"Could not change task's status: {ex.Message}");
            }
        }
    }
}
