using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces;

namespace EmployeesTasksTracker.TasksTrackerService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPdfReportGenerator _reportGenerator;

        public TasksController(IMediator mediator, IPdfReportGenerator reportGenerator)
        {
            _mediator = mediator;
            _reportGenerator = reportGenerator;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllTasks(
            Guid? employeeId,
            Guid? tasksGroupId,
            Guid? projectId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken token = default)
        {
            if (employeeId.HasValue || tasksGroupId.HasValue || projectId.HasValue)
            {
                var result = await _mediator.Send(new GetAllTasksQuery(employeeId, tasksGroupId, projectId), token);

                return Ok(result);
            }

            var tasks = await _mediator.Send(new GetAllTasksPagedQuery(page, pageSize), token);

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id, CancellationToken token)
        {
            var task = await _mediator.Send(new GetTaskByIdQuery(id), token);

            return Ok(task);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command, CancellationToken token)
        {
            var id = await _mediator.Send(command, token);

            var tasks = new { id, command };

            return CreatedAtAction(nameof(GetTaskById), new { id }, tasks);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> EditTask(Guid id, EditTaskDTO editTaskDTO, CancellationToken token)
        {
            await _mediator.Send(new EditTaskCommand(editTaskDTO), token);

            return Ok(editTaskDTO);
        }

        [HttpPost("AddPerformer/")]
        public async Task<IActionResult> AddTaskPerformer(Guid performerId, Guid taskId, CancellationToken token)
        {
            await _mediator.Send(new AddTaskPerformerCommand(performerId, taskId), token);

            return Ok($"Successfully added performer with id - {performerId}, to task with id {taskId}!");
        }

        [HttpPost("AddObserver/")]
        public async Task<IActionResult> AddTaskObserver(Guid observerId, Guid taskId, CancellationToken token)
        {
            await _mediator.Send(new AddTaskObserverCommand(observerId, taskId), token);

            return Ok($"Successfully added observer with id - {observerId}, to task with id {taskId}!");
        }

        [HttpPost("ChangeStatus/")]
        public async Task<IActionResult> ChangeTaskStatus(Guid taskId, string newStatus, CancellationToken token)
        {
            await _mediator.Send(new ChangeTaskStatusCommand(taskId, newStatus), token);

            return Ok($"Successfully changed status to - {newStatus}, for task with id {taskId}!");
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteTask(Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new DeleteTaskCommand(id), token);

            return Ok($"Successfully deleted task with id: {id}");
        }

        [HttpGet("GetTasksByGroupId")]
        public async Task<IActionResult> GetTasksByGroupId(Guid tasksGroupId, CancellationToken token)
        {
            var tasks = await _mediator.Send(new GetTasksByGroupIdQuery(tasksGroupId), token);

            return Ok(tasks);
        }

        [HttpGet("GetProjectId")]
        public async Task<IActionResult> GetProjectId(Guid tasksGroupId, CancellationToken token)
        {
            var projectId = await _mediator.Send(new GetProjectIdQuery(tasksGroupId), token);

            return Ok(projectId);
        }

        [HttpGet("GetAllTasksIds")]
        public async Task<IActionResult> GetAllTasksIds(CancellationToken token)
        {
            var tasks = await _mediator.Send(new GetAllTasksIdsQuery(), token);

            return Ok(tasks.Take(100));
        }

        [HttpGet("GenerateReport/")]
        public async Task<IActionResult> GenerateReport(Guid Id, CancellationToken token)
        {
            var pdfBytes = await _reportGenerator.GenerateReportAsync(Id, token);

            var fileName = $"task_report_{Id}_{DateTime.Now:yyyyMMddHHmm}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }
    }
}
