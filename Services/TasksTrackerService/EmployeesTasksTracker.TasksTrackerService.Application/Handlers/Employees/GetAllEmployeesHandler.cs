using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Employees;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Employees;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.EmployeesService.Application.Handlers
{
    public class GetAllEmployeesHandler : IRequestHandler<GetAllEmployeesQuery, IEnumerable<EmployeeDTO>>
    {

        private readonly IEmployeesRepo _repo;

        public GetAllEmployeesHandler(IEmployeesRepo repo)
        {
            _repo = repo;
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

            return employeesDtoList;

        }
    }
}
