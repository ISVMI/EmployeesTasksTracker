using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Employees;
using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Employees;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Employees;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace EmployeesTasksTracker.TasksTrackerService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllEmployees([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken token = default)
        {
            var employees = await _mediator.Send(new GetAllEmployeesPagedQuery(page, pageSize), token);

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(Guid id, bool infoRequested, CancellationToken token)
        {
            var employee = await _mediator.Send(new GetEmployeeByIdQuery(id), token);

            if (infoRequested)
            {
                var employeeInfo = new EmployeeForReportDTO
                {
                    Name = employee.Name,
                    Surname = employee.Surname,
                    Patronymic = employee.Patronymic,
                    Role = employee.Role.ToString(),
                };

                return Ok(employeeInfo);
            }

            return Ok(employee);

        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeCommand command, CancellationToken token)
        {
            var id = await _mediator.Send(command, token);

            var employee = new { id, command };

            return CreatedAtAction(nameof(GetEmployeeById), new { id }, employee);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> EditEmployee(Guid id, EditEmployeeDTO editEmployeeDto, CancellationToken token)
        {
            editEmployeeDto.Id = id;

            await _mediator.Send(new EditEmployeeCommand(editEmployeeDto), token);

            return Ok(editEmployeeDto);

        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id, CancellationToken token)
        {
            await _mediator.Send(new DeleteEmployeeCommand(id), token);

            return Ok($"Successfully deleted employee with id {id}");
        }

        [HttpGet("GetAllEmployeesIds")]
        public async Task<IActionResult> GetAllIds(CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllEmployeesIdsQuery(), token);

            return Ok(result.Take(100));
        }

    }
}
