using EmployeesTasksTracker.TasksService.Application.Commands;
using EmployeesTasksTracker.TasksService.Application.DTOs;
using EmployeesTasksTracker.TasksService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesTasksTracker.TasksService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllTasks(Guid? employeeId, Guid? tasksGroupId, Guid? projectId, CancellationToken token)
        {
            var tasks = await _mediator.Send(new GetAllTasksQuery(employeeId, tasksGroupId, projectId), token);

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id, CancellationToken token)
        {
            try
            {
                var tasks = await _mediator.Send(new GetTaskByIdQuery(id), token);

                return Ok(tasks);
            }
            catch (Exception ex)
            {
                var message = $"Could not find task with the given id {id} : {ex.Message}";

                Console.WriteLine(message);

                return NotFound(new { message });
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command, CancellationToken token)
        {
            try
            {
                var id = await _mediator.Send(command, token);

                var tasks = new { id, command };

                return CreatedAtAction(nameof(GetTaskById), new { id }, tasks);
            }
            catch (Exception ex)
            {
                var message = $"Could not create task: {ex.Message}";

                Console.WriteLine(message);

                return BadRequest(new { message, command });
            }
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> EditTask(Guid id, EditTaskDTO editTaskDTO, CancellationToken token)
        {

            editTaskDTO.Id = id;

            try
            {
                await _mediator.Send(new EditTaskCommand(editTaskDTO), token);

                return Ok(editTaskDTO);
            }
            catch (Exception ex)
            {
                var message = $"Could not edit task : {ex.Message} / {ex.InnerException?.Message}";

                Console.WriteLine(message);

                return BadRequest(new { id, message });
            }
        }

        [HttpPost("AddPerformer")]
        public async Task<IActionResult> AddTaskPerformer(Guid performerId, Guid taskId, CancellationToken token)
        {

            try
            {
                await _mediator.Send(new AddTaskPerformerCommand(performerId, taskId), token);

                return Ok($"Successfully added performer with id - {performerId}, to task with id {taskId}!");
            }
            catch (Exception ex)
            {
                var message = $"Could not add performer to task : {ex.Message}";

                Console.WriteLine(message);

                return BadRequest(new { performerId, taskId, message });
            }
        }

        [HttpPost("AddObserver")]
        public async Task<IActionResult> AddTaskObserver(Guid observerId, Guid taskId, CancellationToken token)
        {

            try
            {
                await _mediator.Send(new AddTaskPerformerCommand(observerId, taskId), token);

                return Ok($"Successfully added observer with id - {observerId}, to task with id {taskId}!");
            }
            catch (Exception ex)
            {
                var message = $"Could not add observer to task : {ex.Message}";

                Console.WriteLine(message);

                return BadRequest(new { observerId, taskId, message });
            }
        }

        [HttpPost("ChangeStatus/{taskId}")]
        public async Task<IActionResult> ChangeTaskStatus(Guid taskId, string newStatus, CancellationToken token)
        {

            try
            {
                await _mediator.Send(new ChangeTaskStatusCommand(taskId, newStatus), token);

                return Ok($"Successfully changed status to - {newStatus}, for task with id {taskId}!");
            }
            catch (Exception ex)
            {
                var message = $"Could not change task's status : {ex.Message}";

                Console.WriteLine(message);

                return BadRequest(new { taskId, newStatus,  message });
            }
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteTask(Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new DeleteTaskCommand(id), token);

            if (result == false)
            {
                var message = $"Could not delete task with id {id}";

                return NotFound(new { message });
            }

            return Ok($"Successfully deleted task with id {id}");
        }
    }
}
