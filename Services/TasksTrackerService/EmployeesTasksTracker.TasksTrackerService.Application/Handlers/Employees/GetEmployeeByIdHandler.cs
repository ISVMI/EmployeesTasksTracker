using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Employees;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Employees;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Employees
{
    public class GetEmployeeByIdHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDTO>
    {
        private readonly IEmployeesRepo _repo;

        public GetEmployeeByIdHandler(IEmployeesRepo repo)
        {
            _repo = repo;
        }

        public async Task<EmployeeDTO> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
                var employee = await _repo.GetByIdAsync(request.Id, cancellationToken);

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
