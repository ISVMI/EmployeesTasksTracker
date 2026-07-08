using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Employees;
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Employees
{
    public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, Guid>
    {
        private readonly IEmployeesRepo _repo;
        private readonly ILogger _logger;

        public CreateEmployeeHandler(IEmployeesRepo repo, ILogger<CreateEmployeeHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {

            if (!Enum.TryParse<EmployeeRole>(request.Employee.Role, true, out EmployeeRole employeeRole))
            {
                throw new DomainException($"Unknown status {request.Employee.Role}");
            }

            var employee = new Employee
            {
                Name = request.Employee.Name,
                Surname = request.Employee.Surname,
                Patronymic = request.Employee.Patronymic,
                Role = employeeRole,
                UserName = request.Employee.UserName
            };

            var result = await _repo.CreateAsync(employee, cancellationToken);

            _logger.LogInformation("New employee created with id {EmployeeId}", result);

            return result;
        }
    }
}
