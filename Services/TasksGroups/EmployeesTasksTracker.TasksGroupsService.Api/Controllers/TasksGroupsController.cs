using MediatR;
using Microsoft.AspNetCore.Mvc;
using EmployeesTasksTracker.TasksGroupsService.Application.Commands;
using EmployeesTasksTracker.TasksGroupsService.Application.Queries;
using EmployeesTasksTracker.TasksGroupsService.Application.DTOs;

namespace EmployeesTasksTracker.TasksGroupsService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksGroupsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksGroupsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllTasksGroups(CancellationToken token)
        {
            var tasksGroups = await _mediator.Send(new GetAllTasksGroupsQuery(), token);

            return Ok(tasksGroups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTasksGroupById(Guid id, CancellationToken token)
        {
            try
            {
                var tasksGroup = await _mediator.Send(new GetTasksGroupByIdQuery(id), token);

                return Ok(tasksGroup);
            }
            catch (Exception ex)
            {
                var message = $"Could not find tasks group with the given id {id} : {ex.Message}";

                Console.WriteLine(message);

                return NotFound(new { message });
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateTasksGroup([FromBody] CreateTasksGroupCommand command, CancellationToken token)
        {
            try
            {
                var id = await _mediator.Send(command, token);

                var tasksGroup = new { id, command };

                return CreatedAtAction(nameof(GetTasksGroupById), new { id }, tasksGroup);
            }
            catch (Exception ex)
            {
                var message = $"Could not create tasks group: {ex.Message}";

                Console.WriteLine(message);

                return BadRequest(new { message, command });
            }
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> EditTasksGroup(Guid id, EditTasksGroupDTO editTasksGroupDTO, CancellationToken token)
        {

            editTasksGroupDTO.Id = id;

            try
            {
                await _mediator.Send(new EditTasksGroupCommand(editTasksGroupDTO), token);

                return Ok(editTasksGroupDTO);
            }
            catch (Exception ex)
            {
                var message = $"Could not edit tasks group : {ex.Message} / {ex.InnerException?.Message}";

                Console.WriteLine(message);

                return BadRequest(new { id, message });
            }
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteTasksGroup(Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new DeleteTasksGroupCommand(id), token);

            if (result == false)
            {
                var message = $"Could not delete tasks group with id {id}";

                return NotFound(new { message });
            }

            return Ok($"Successfully deleted tasks group with id {id}");
        }

        [HttpGet("GetAllTaskGroupsIds")]
        public async Task<IActionResult> GetAllIds(CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllTasksGroupsIdsQuery(), token);

            return Ok(result);
        }
    }
}
