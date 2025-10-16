using AutoMapper;
using EmployeesTasksTracker.EmployeesService.Application.Commands;
using EmployeesTasksTracker.EmployeesService.Application.DTOs;
using EmployeesTasksTracker.EmployeesService.Core.Interfaces;
using EmployeesTasksTracker.EmployeesService.Core.Models;
using MediatR;

namespace EmployeesTasksTracker.EmployeesService.Application.Handlers
{
    public class EditEmployeeHandler : IRequestHandler<EditEmployeeCommand, EmployeeDTO>
    {
        private readonly IEmployeesRepo _repo;
        private readonly IMapper _mapper;

        public EditEmployeeHandler(IEmployeesRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<EmployeeDTO> Handle(EditEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = _mapper.Map<Employee>(request.EmployeeToEdit);

            await _repo.UpdateAsync(employee, cancellationToken);

            return _mapper.Map<EmployeeDTO>(employee);
        }
    }
}
