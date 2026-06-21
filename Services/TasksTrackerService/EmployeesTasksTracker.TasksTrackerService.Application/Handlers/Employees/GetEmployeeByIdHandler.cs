using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Employees;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Employees;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Employees
{
    public class GetEmployeeByIdHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDTO>
    {
        private readonly IEmployeesRepo _repo;
        private readonly HybridCache _cache;
        private readonly ILogger<GetEmployeeByIdHandler> _logger;

        public GetEmployeeByIdHandler(IEmployeesRepo repo, HybridCache cache, ILogger<GetEmployeeByIdHandler> logger)
        {
            _repo = repo;
            _cache = cache;
            _logger = logger;
        }

        public async Task<EmployeeDTO> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {

            var cacheKey = $"employee:{request.Id}";

            var employee = await _cache.GetOrCreateAsync(
                            cacheKey,
                            async token => await _repo.GetByIdAsync(request.Id, cancellationToken),
                            cancellationToken: cancellationToken) ?? throw new NotFoundException("employee", request.Id);

            _logger.LogInformation("Found employee {Surname} {Name} {Patronymic}", employee.Surname, employee.Name, employee.Patronymic);

            return new EmployeeDTO
            {
                Name = employee.Name,
                Surname = employee.Surname,
                Patronymic = employee.Patronymic,
                Role = employee.Role.ToString(),
                UserName = employee.UserName
            };
        }
    }
}
