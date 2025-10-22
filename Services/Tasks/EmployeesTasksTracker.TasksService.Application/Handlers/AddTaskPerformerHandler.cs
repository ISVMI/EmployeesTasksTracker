using EmployeesTasksTracker.TasksService.Application.Commands;
using EmployeesTasksTracker.TasksService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksService.Application.Handlers
{
    public class AddTaskPerformerHandler : IRequestHandler<AddTaskPerformerCommand>
    {
        private readonly ITasksRepo _repo;

        public AddTaskPerformerHandler(ITasksRepo repo)
        {
            _repo = repo;
        }

        public async Task Handle(AddTaskPerformerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _repo.AddPerformerAsync(request.PerformerId, request.TaskId, cancellationToken);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Could not add performer: {ex.Message}");
            }
        }
    }
}
