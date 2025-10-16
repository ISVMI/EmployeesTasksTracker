
using AutoMapper;
using EmployeesTasksTracker.EmployeesService.Application.DTOs;
using EmployeesTasksTracker.EmployeesService.Application.Queries;
using EmployeesTasksTracker.EmployeesService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.EmployeesService.Application.Handlers
{
    public class GetEmployeeByIdHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDTO>
    {
        private readonly IEmployeesRepo _repo;
        private readonly IMapper _mapper;

        public GetEmployeeByIdHandler(IEmployeesRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<EmployeeDTO> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _repo.GetByIdAsync(request.Id, cancellationToken);

                return _mapper.Map<EmployeeDTO>(employee);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Could not get employee by id {request.Id} : {ex.Message}");

                return new EmployeeDTO();
            }
        }
    }
}
