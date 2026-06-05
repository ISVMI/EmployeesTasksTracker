using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Employees;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Employees;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Extensions;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Employees
{
    public class GetEmployeeByIdHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDTO>
    {
        private readonly IEmployeesRepo _repo;
        private readonly IDistributedCache _cache;

        public GetEmployeeByIdHandler(IEmployeesRepo repo, IDistributedCache cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public async Task<EmployeeDTO> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {

            var cacheKey = $"employee:{request.Id}";

            var cachedEmployee = await _cache.GetRecordAsync<Employee>(cacheKey);

            var employee = cachedEmployee ?? await _repo.GetByIdAsync(request.Id, cancellationToken);

            if (cachedEmployee == null)
            {
                var expirationTime = TimeSpan.FromMinutes(30);

                await _cache.SetRecordAsync(cacheKey, employee, expirationTime);
            }

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
