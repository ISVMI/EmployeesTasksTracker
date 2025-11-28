using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Employees;
using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Employees;
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Employees
{
    public class EditEmployeeHandler : IRequestHandler<EditEmployeeCommand, EmployeeDTO>
    {
        private readonly IEmployeesRepo _repo;

        public EditEmployeeHandler(IEmployeesRepo repo)
        {
            _repo = repo;
        }

        public async Task<EmployeeDTO> Handle(EditEmployeeCommand request, CancellationToken cancellationToken)
        {

            var employee = new Employee
            {
                Name = request.EmployeeToEdit.Name,
                Surname = request.EmployeeToEdit.Surname,
                Patronymic = request.EmployeeToEdit.Patronymic,
                Role = EmployeeRole.QA, //request.EmployeeToEdit.Role,
                UserName = request.EmployeeToEdit.UserName,
            };

            await _repo.UpdateAsync(employee, cancellationToken);

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
