using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Employees;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Employees
{
    public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeCommand, bool>
    {
        private readonly IEmployeesRepo _repo;
        private readonly HybridCache _cache;
        private readonly ILogger<DeleteEmployeeHandler> _logger;

        public DeleteEmployeeHandler(IEmployeesRepo repo, HybridCache cache, ILogger<DeleteEmployeeHandler> logger)
        {
            _repo = repo;
            _cache = cache;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            await _cache.RemoveAsync($"employee:{request.Id}", cancellationToken);

            var result = await _repo.DeleteAsync(request.Id, cancellationToken);

            _logger.LogInformation("Successfully deleted employee with id {EmployeeId}", request.Id);

            return result;
        }
    }
}
