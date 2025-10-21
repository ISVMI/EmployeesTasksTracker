using EmployeesTasksTracker.ProjectsService.Application.Commands;
using EmployeesTasksTracker.ProjectsService.Application.DTOs;
using EmployeesTasksTracker.ProjectsService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProjectsTasksTracker.ProjectsService.Api.Controllers
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
        public async Task<IActionResult> GetAllProjects(CancellationToken token)
        {
            var Projects = await _mediator.Send(new GetAllProjectsQuery(), token);

            return Ok(Projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(Guid id, CancellationToken token)
        {
            try
            {
                var Project = await _mediator.Send(new GetProjectByIdQuery(id), token);

                return Ok(Project);
            }
            catch (Exception ex)
            {
                var message = $"Could not find project with the given id {id} : {ex.Message}";

                Console.WriteLine(message);

                return NotFound(new { message });
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand command, CancellationToken token)
        {
            try
            {
                var id = await _mediator.Send(command, token);

                var project = new { id, command };

                return CreatedAtAction(nameof(GetProjectById), new { id }, command);
            }
            catch (Exception ex)
            {
                var message = $"Could not create project: {ex.Message}";

                Console.WriteLine(message);

                return BadRequest(new { message, command });
            }
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> EditProject(Guid id, EditProjectDTO editProjectDto, CancellationToken token)
        {

            editProjectDto.Id = id;

            try
            {
                await _mediator.Send(new EditProjectCommand(editProjectDto), token);

                return Ok(editProjectDto);
            }
            catch (Exception ex)
            {
                var message = $"Could not edit project : {ex.Message} / {ex.InnerException?.Message}";

                Console.WriteLine(message);

                return BadRequest(new { id, message });
            }
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteProject(Guid id, CancellationToken token)
        {
            var result = await _mediator.Send(new DeleteProjectCommand(id), token);

            if (result == false)
            {
                var message = $"Could not delete project with id {id}";

                return NotFound(new { message });
            }

            return Ok($"Successfully deleted project with id {id}");
        }
    }
}
