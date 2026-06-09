using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Employees;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Employees;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Employees
{
    public class GetEmployeeByIdHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDTO>
    {
        private readonly IEmployeesRepo _repo;
        private readonly HybridCache _cache;

        public GetEmployeeByIdHandler(IEmployeesRepo repo, HybridCache cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public async Task<EmployeeDTO> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {

            var cacheKey = $"employee:{request.Id}";

            var employee = await _cache.GetOrCreateAsync(
                            cacheKey,
                            async token => await _repo.GetByIdAsync(request.Id, cancellationToken),
                            cancellationToken: cancellationToken);

            return employee is null
                ? throw new NotFoundException("employee", request.Id)
                : new EmployeeDTO
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
