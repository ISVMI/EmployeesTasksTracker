using EmployeesTasksTracker.EmployeesService.Application.Commands;
using EmployeesTasksTracker.EmployeesService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.EmployeesService.Application.Handlers
{
    public class DeleteEmployeeHandler : IRequestHandler<DeleteEmployeeCommand, bool>
    {
        private readonly IEmployeesRepo _repo;

        public DeleteEmployeeHandler(IEmployeesRepo repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            return await _repo.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
