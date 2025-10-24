using EmployeesTasksTracker.TasksService.Application.Commands;
using EmployeesTasksTracker.TasksService.Application.DTOs;
using EmployeesTasksTracker.TasksService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesTasksTracker.TasksService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
                var task = await _mediator.Send(new GetTaskByIdQuery(id), token);

                return Ok(task);
            }
            catch (Exception ex)
            {
                var message = $"Could not find task with the given id {id} : {ex.Message}";

                var problem = new ProblemDetails
                {
                    Title = "Task not found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = message,
                    Instance = HttpContext.Request.Path
                };

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

                var problem = new ProblemDetails
                {
                    Title = "Couldn't create task",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = ex.Message,
                    Instance = HttpContext.Request.Path
                };

                Console.WriteLine(message);

                return BadRequest(new { problem, command });
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

                var problem = new ProblemDetails
                {
                    Title = "Couldn't edit task",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = $"{ex.Message} {ex.InnerException?.Message}",
                    Instance = HttpContext.Request.Path,
                    Extensions =
                    {
                        ["taskId"] = id
                    }
                };

                Console.WriteLine(message);

                return BadRequest(problem);
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

                var problem = new ProblemDetails
                {
                    Title = "Couldn't add performer",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = message,
                    Instance = HttpContext.Request.Path,
                    Extensions =
                    {
                        ["performerId"] = performerId,
                        ["taskId"] = taskId
                    }
                };

                Console.WriteLine(message);

                return BadRequest(problem);
            }
        }

        [HttpPost("AddObserver")]
        public async Task<IActionResult> AddTaskObserver(Guid observerId, Guid taskId, CancellationToken token)
        {

            try
            {
                await _mediator.Send(new AddTaskObserverCommand(observerId, taskId), token);

                return Ok($"Successfully added observer with id - {observerId}, to task with id {taskId}!");
            }
            catch (Exception ex)
            {
                var message = $"Could not add observer to task : {ex.Message}";

                var problem = new ProblemDetails
                {
                    Title = "Couldn't add observer",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = message,
                    Instance = HttpContext.Request.Path,
                    Extensions =
                    {
                        ["observerId"] = observerId,
                        ["taskId"] = taskId
                    }
                };

                Console.WriteLine(message);

                return BadRequest(problem);
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

                var problem = new ProblemDetails
                {
                    Title = "Couldn't change status",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = message,
                    Instance = HttpContext.Request.Path,
                    Extensions =
                    {
                        ["taskId"] = taskId,
                        ["newStatus"] = newStatus
                    }
                };

                Console.WriteLine(message);

                return BadRequest(problem);
            }
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteTask(Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new DeleteTaskCommand(id), token);

            if (result == false)
            {
                var message = $"Could not delete task with id {id}";

                var problem = new ProblemDetails
                {
                    Title = "Couldn't delete task",
                    Status = StatusCodes.Status404NotFound,
                    Detail = message,
                    Instance = HttpContext.Request.Path,
                    Extensions =
                    {
                        ["taskId"] = id
                    }
                };

                return NotFound(problem);
            }

            return Ok($"Successfully deleted task with id: {id}");
        }
    }
}
