using MediatR;
using Microsoft.AspNetCore.Mvc;
using EmployeesTasksTracker.TasksGroupsService.Application.Commands;
using EmployeesTasksTracker.TasksGroupsService.Application.Queries;
using EmployeesTasksTracker.TasksGroupsService.Application.DTOs;
using Shared.Interfaces;

namespace EmployeesTasksTracker.TasksGroupsService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksGroupsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPdfReportGenerator _reportGenerator;

        public TasksGroupsController(IMediator mediator, IPdfReportGenerator reportGenerator)
        {
            _mediator = mediator;
            _reportGenerator = reportGenerator;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllTasksGroups(CancellationToken token)
        {
            var tasksGroups = await _mediator.Send(new GetAllTasksGroupsQuery(), token);

            return Ok(tasksGroups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTasksGroupById(Guid id, bool nameRequested, CancellationToken token)
        {
            var tasksGroup = await _mediator.Send(new GetTasksGroupByIdQuery(id), token);

            if (nameRequested)
            {
                return Ok(tasksGroup.Name);
            }

            return Ok(tasksGroup);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateTasksGroup([FromBody] CreateTasksGroupCommand command, CancellationToken token)
        {
            var id = await _mediator.Send(command, token);

            var tasksGroup = new { id, command };

            return CreatedAtAction(nameof(GetTasksGroupById), new { id }, tasksGroup);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> EditTasksGroup(Guid id, EditTasksGroupDTO editTasksGroupDTO, CancellationToken token)
        {

            editTasksGroupDTO.Id = id;

            await _mediator.Send(new EditTasksGroupCommand(editTasksGroupDTO), token);

            return Ok(editTasksGroupDTO);

        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteTasksGroup(Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new DeleteTasksGroupCommand(id), token);

            if (result == false)
            {
                var message = $"Could not delete tasks group with id {id}";

                var problem = new ProblemDetails
                {
                    Title = "Couldn't delete tasks group",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = message,
                    Instance = HttpContext.Request.Path,
                    Extensions =
                    {
                        ["tasksGroupId"] = id
                    }
                };

                return NotFound(problem);
            }

            return Ok($"Successfully deleted tasks group with id {id}");
        }

        [HttpGet("GetAllTasksGroupsIds")]
        public async Task<IActionResult> GetAllIds(CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllTasksGroupsIdsQuery(), token);

            return Ok(result);
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
