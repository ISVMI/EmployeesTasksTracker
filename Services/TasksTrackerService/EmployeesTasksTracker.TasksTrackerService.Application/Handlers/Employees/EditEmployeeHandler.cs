using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Employees;
using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Employees;
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Employees
{
    public class EditEmployeeHandler : IRequestHandler<EditEmployeeCommand, EmployeeDTO>
    {
        private readonly IEmployeesRepo _repo;
        private readonly IDistributedCache _cache;

        public EditEmployeeHandler(IEmployeesRepo repo, IDistributedCache cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public async Task<EmployeeDTO> Handle(EditEmployeeCommand request, CancellationToken cancellationToken)
        {

            if (!Enum.TryParse<EmployeeRole>(request.EmployeeToEdit.Role, true, out EmployeeRole employeeRole))
            {
                throw new DomainException($"Unknown status {request.EmployeeToEdit.Role}");
            }

            var employee = new Employee
            {
                Name = request.EmployeeToEdit.Name,
                Surname = request.EmployeeToEdit.Surname,
                Patronymic = request.EmployeeToEdit.Patronymic,
                Role = employeeRole,
                UserName = request.EmployeeToEdit.UserName,
            };

            await _repo.UpdateAsync(employee, cancellationToken);

            await _cache.RemoveAsync($"employee:{request.EmployeeToEdit.Id}", cancellationToken);

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
