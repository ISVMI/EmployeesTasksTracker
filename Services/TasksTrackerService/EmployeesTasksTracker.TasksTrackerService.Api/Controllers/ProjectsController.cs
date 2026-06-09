using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Projects;
using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Projects;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Projects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesTasksTracker.TasksTrackerService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllProjects([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken token = default)
        {
            var Projects = await _mediator.Send(new GetAllProjectsPagedQuery(page, pageSize), token);

            return Ok(Projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(Guid id, bool nameRequested, CancellationToken token)
        {
            var Project = await _mediator.Send(new GetProjectByIdQuery(id), token);

            if (nameRequested)
            {
                return Ok(Project.Name);
            }

            return Ok(Project);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand command, CancellationToken token)
        {
            var id = await _mediator.Send(command, token);

            var project = new { id, command };

            return CreatedAtAction(nameof(GetProjectById), new { id }, project);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> EditProject(Guid id, EditProjectDTO editProjectDto, CancellationToken token)
        {

            editProjectDto.Id = id;

            await _mediator.Send(new EditProjectCommand(editProjectDto), token);

            return Ok(editProjectDto);
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteProject(Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new DeleteProjectCommand(id), token);

            return Ok($"Successfully deleted project with id {id}");
        }

        [HttpGet("GetAllProjectsIds")]
        public async Task<IActionResult> GetAllIds(CancellationToken token)
        {
            var result = await _mediator.Send(new GetAllProjectsIdsQuery(), token);

            return Ok(result.Take(100));
        }
    }
}
