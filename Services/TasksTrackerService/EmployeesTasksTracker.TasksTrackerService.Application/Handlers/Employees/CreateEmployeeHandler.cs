using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Employees;
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using MediatR;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Employees
{
    public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, Guid>
    {
        private readonly IEmployeesRepo _repo;

        public CreateEmployeeHandler(IEmployeesRepo repo)
        {
            _repo = repo;
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

                return result;
        }
    }
}
