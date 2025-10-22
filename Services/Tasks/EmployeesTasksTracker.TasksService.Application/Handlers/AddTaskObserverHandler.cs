using EmployeesTasksTracker.TasksService.Application.Commands;
using EmployeesTasksTracker.TasksService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksService.Application.Handlers
{
    public class AddTaskObserverHandler : IRequestHandler<AddTaskObserverCommand>
    {
        private readonly ITasksRepo _repo;

        public AddTaskObserverHandler(ITasksRepo repo)
        {
            _repo = repo;
        }

        public async Task Handle(AddTaskObserverCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _repo.AddObserverAsync(request.ObserverId, request.TaskId, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not add observer: {ex.Message}");
            }
        }
    }
}
