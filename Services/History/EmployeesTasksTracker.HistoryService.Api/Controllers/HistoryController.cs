using EmployeesTasksTracker.HistoryService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesTasksTracker.HistoryService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HistoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllTasksChanges(CancellationToken token)
        {
            var allTasksChanges = await _mediator.Send(new GetAllTasksChangesQuery(), token);

            return Ok(allTasksChanges);
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTaskById(Guid taskId, CancellationToken token)
        {
            try
            {
                var taskChanges = await _mediator.Send(new GetTaskChangesByTaskIdQuery(taskId), token);

                return Ok(taskChanges);
            }
            catch (Exception ex)
            {
                var message = $"Could not find changes for task with id - {taskId} : {ex.Message}";

                var problem = new ProblemDetails
                {
                    Title = "Changes not found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = message,
                    Instance = HttpContext.Request.Path
                };

                Console.WriteLine(message);

                return NotFound(problem);
            }
        }
    }
}
