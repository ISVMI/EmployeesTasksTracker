using AutoMapper;
using EmployeesTasksTracker.EmployeesService.Application.DTOs;
using EmployeesTasksTracker.EmployeesService.Application.Queries;
using EmployeesTasksTracker.EmployeesService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.EmployeesService.Application.Handlers
{
    public class GetAllEmployeesHandler : IRequestHandler<GetAllEmployeesQuery, IEnumerable<EmployeeDTO>>
    {

        private readonly IEmployeesRepo _repo;
        private readonly IMapper _mapper;

        public GetAllEmployeesHandler(IEmployeesRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<EmployeeDTO>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            var employees = await _repo.GetAllAsync(cancellationToken);

            return _mapper.Map<IEnumerable<EmployeeDTO>>(employees);

        }
    }
}
