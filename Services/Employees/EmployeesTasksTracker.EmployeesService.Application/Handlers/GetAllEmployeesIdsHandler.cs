using AutoMapper;
using EmployeesTasksTracker.EmployeesService.Application.DTOs;
using EmployeesTasksTracker.EmployeesService.Application.Queries;
using EmployeesTasksTracker.EmployeesService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.EmployeesService.Application.Handlers
{
    public class GetAllEmployeesIdsHandler : IRequestHandler<GetAllEmployeesIdsQuery, IEnumerable<Guid>>
    {

        private readonly IEmployeesRepo _repo;

        public GetAllEmployeesIdsHandler(IEmployeesRepo repo, IMapper mapper)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<Guid>> Handle(GetAllEmployeesIdsQuery request, CancellationToken cancellationToken)
        {
            var employeesIds = await _repo.GetAllIdsAsync();

            return employeesIds;
        }
    }
}
