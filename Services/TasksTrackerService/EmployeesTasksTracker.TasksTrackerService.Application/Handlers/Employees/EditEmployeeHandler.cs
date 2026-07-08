using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Employees;
using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Employees;
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Employees
{
    public class EditEmployeeHandler : IRequestHandler<EditEmployeeCommand, EmployeeDTO>
    {
        private readonly IEmployeesRepo _repo;
        private readonly HybridCache _cache;
        private readonly ILogger<EditEmployeeHandler> _logger;

        public EditEmployeeHandler(IEmployeesRepo repo, HybridCache cache, ILogger<EditEmployeeHandler> logger)
        {
            _repo = repo;
            _cache = cache;
            _logger = logger;
        }

        public async Task<EmployeeDTO> Handle(EditEmployeeCommand request, CancellationToken cancellationToken)
        {

            if (!Enum.TryParse<EmployeeRole>(request.EmployeeToEdit.Role, true, out EmployeeRole employeeRole))
            {
                throw new DomainException($"Unknown status {request.EmployeeToEdit.Role}");
            }

            await _cache.RemoveAsync($"employee:{request.EmployeeToEdit.Id}", cancellationToken);

            var employeeToEdit = await _repo.GetByIdAsync(request.EmployeeToEdit.Id, cancellationToken)
                ?? throw new NotFoundException("employee", request.EmployeeToEdit.Id);

            employeeToEdit.Name = request.EmployeeToEdit.Name;
            employeeToEdit.Surname = request.EmployeeToEdit.Surname;
            employeeToEdit.Patronymic = request.EmployeeToEdit.Patronymic;
            employeeToEdit.Role = employeeRole;
            employeeToEdit.UserName = request.EmployeeToEdit.UserName;


            var employee = await _repo.UpdateAsync(employeeToEdit, cancellationToken) ?? throw new NotFoundException("employee", request.EmployeeToEdit.Id);

            _logger.LogInformation("Successfully edited employee with id {EmployeeId}", employee.Id);

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
