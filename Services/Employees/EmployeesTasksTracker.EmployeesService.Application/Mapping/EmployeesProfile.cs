using AutoMapper;
using EmployeesTasksTracker.EmployeesService.Application.DTOs;
using EmployeesTasksTracker.EmployeesService.Core.Models;

namespace EmployeesTasksTracker.EmployeesService.Application.Mapping
{
    public class EmployeesProfile : Profile
    {
        public EmployeesProfile()
        {
            CreateMap<Employee, EmployeeDTO>();
            CreateMap<EmployeeDTO, Employee>();
            CreateMap<EditEmployeeDTO, Employee>();
        }
    }
}
