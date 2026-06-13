using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Employees;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Employees;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Employees
{
    public class GetAllEmployeesHandler : IRequestHandler<GetAllEmployeesQuery, IEnumerable<EmployeeDTO>>
    {

        private readonly IEmployeesRepo _repo;
        private readonly ILogger<GetAllEmployeesHandler> _logger;

        public GetAllEmployeesHandler(IEmployeesRepo repo, ILogger<GetAllEmployeesHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        public async Task<IEnumerable<EmployeeDTO>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            var employees = await _repo.GetAllAsync(cancellationToken);

            var employeesDtoList = new List<EmployeeDTO>();

            foreach (var employee in employees) 
            {
                employeesDtoList.Add(new EmployeeDTO
                {
                    Name = employee.Name,
                    Surname = employee.Surname,
                    Patronymic = employee.Patronymic,
                    Role = employee.Role.ToString(),
                    UserName = employee.UserName
                });
            }

            _logger.LogInformation("Successfully got {totalCount} employees", employees.Count());

            return employeesDtoList;

        }
    }
}
