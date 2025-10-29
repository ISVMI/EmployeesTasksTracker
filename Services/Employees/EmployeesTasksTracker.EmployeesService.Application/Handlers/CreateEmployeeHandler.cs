using AutoMapper;
using EmployeesTasksTracker.EmployeesService.Application.Commands;
using EmployeesTasksTracker.EmployeesService.Core.Interfaces;
using EmployeesTasksTracker.EmployeesService.Core.Models;
using MediatR;

namespace EmployeesTasksTracker.EmployeesService.Application.Handlers
{
    public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, Guid>
    {
        private readonly IEmployeesRepo _repo;
        private readonly IMapper _mapper;

        public CreateEmployeeHandler(IEmployeesRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = _mapper.Map<Employee>(request.Employee);

                var result = await _repo.CreateAsync(employee, cancellationToken);

                return result;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
