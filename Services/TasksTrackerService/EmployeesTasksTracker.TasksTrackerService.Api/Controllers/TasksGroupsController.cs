using EmployeesTasksTracker.TasksTrackerService.Application.Commands.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.TasksGroups;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces;

namespace EmployeesTasksTracker.TasksTrackerService.Api.Controllers
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
        public async Task<IActionResult> GetAllTasksGroups([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken token = default)
        {
            var tasksGroups = await _mediator.Send(new GetAllTasksGroupsPagedQuery(page, pageSize), token);

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

            return Ok($"Successfully deleted tasks group with id {id}");
        }

        [HttpGet("GetAllTasksGroupsIds")]
        public async Task<IActionResult> GetAllIds(CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllTasksGroupsIdsQuery(), token);

            return Ok(result.Take(100));
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
