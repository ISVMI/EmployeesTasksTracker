using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesTasksTracker.EmployeesService.Api.Controllers
{
    [ApiController]
    [Route("api/employees/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
