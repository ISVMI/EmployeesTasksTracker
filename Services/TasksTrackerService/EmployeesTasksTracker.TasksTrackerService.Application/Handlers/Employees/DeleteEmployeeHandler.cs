using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Employees;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Employees
{
    public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeCommand, bool>
    {
        private readonly IEmployeesRepo _repo;
        private readonly IDistributedCache _cache;

        public DeleteEmployeeHandler(IEmployeesRepo repo, IDistributedCache cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            await _cache.RemoveAsync($"employee:{request.Id}", cancellationToken);

            return await _repo.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
