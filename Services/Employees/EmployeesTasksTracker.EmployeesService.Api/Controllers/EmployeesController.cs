using EmployeesTasksTracker.EmployeesService.Application.Commands;
using EmployeesTasksTracker.EmployeesService.Application.DTOs;
using EmployeesTasksTracker.EmployeesService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesTasksTracker.EmployeesService.Api.Controllers
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
        public async Task<IActionResult> GetAllEmployees(CancellationToken token)
        {
            var employees = await _mediator.Send(new GetAllEmployeesQuery(), token);

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(Guid id, CancellationToken token)
        {
            try
            {
                var employee = await _mediator.Send(new GetEmployeeByIdQuery(id), token);

                return Ok(employee);
            }
            catch (Exception ex)
            {
                var message = $"Could not find employee with the given id {id} : {ex.Message}";

                var problem = new ProblemDetails
                {
                    Title = "Couldn't find employee",
                    Status = StatusCodes.Status404NotFound,
                    Detail = ex.Message,
                    Instance = HttpContext.Request.Path,
                    Extensions =
                    {
                        ["employeeId"] = id
                    }
                };

                Console.WriteLine(message);

                return NotFound(problem);
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeCommand command, CancellationToken token)
        {
            try
            {
                var id = await _mediator.Send(command, token);

                var employee = new { id, command };

                return CreatedAtAction(nameof(GetEmployeeById), new { id }, employee);
            }
            catch (Exception ex)
            {
                var message = $"Could not create an employee: {ex.Message}";

                var problem = new ProblemDetails
                {
                    Title = "Couldn't create an employee",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = ex.Message,
                    Instance = HttpContext.Request.Path,
                };

                Console.WriteLine(message);

                return BadRequest(new { problem, command });
            }
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> EditEmployee(Guid id, EditEmployeeDTO editEmployeeDto, CancellationToken token)
        {

            editEmployeeDto.Id = id;

            try
            {
                await _mediator.Send(new EditEmployeeCommand(editEmployeeDto), token);

                return Ok(editEmployeeDto);
            }
            catch (Exception ex)
            {
                var message = $"Could not edit employee : {ex.Message} / {ex.InnerException?.Message}";

                var problem = new ProblemDetails
                {
                    Title = "Couldn't edit employee",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = $"{ex.Message} {ex.InnerException?.Message}",
                    Instance = HttpContext.Request.Path,
                    Extensions =
                    {
                        ["employeeId"] = id
                    }
                };

                Console.WriteLine(message);

                return BadRequest(new { id, message });
            }
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new DeleteEmployeeCommand(id), token);

            if (result == false)
            {
                var message = $"Could not delete employee with id {id}";

                var problem = new ProblemDetails
                {
                    Title = "Couldn't not delete employee",
                    Status = StatusCodes.Status404NotFound,
                    Detail = message,
                    Instance = HttpContext.Request.Path,
                    Extensions =
                    {
                        ["employeeId"] = id
                    }
                };

                return NotFound(problem);
            }

            return Ok($"Successfully deleted employee with id {id}");
        }

        [HttpGet("GetAllEmployeesIds")]
        public async Task<IActionResult> GetAllIds(CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllEmployeesIdsQuery(), token);

            return Ok(result);
        }

    }
}
