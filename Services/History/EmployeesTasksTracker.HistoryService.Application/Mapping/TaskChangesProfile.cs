using AutoMapper;
using EmployeesTasksTracker.HistoryService.Application.DTOs;
using EmployeesTasksTracker.HistoryService.Core.Models;

namespace EmployeesTasksTracker.HistoryService.Application.Mapping
{
    public class TaskChangesProfile : Profile
    {
        public TaskChangesProfile() 
        {
            CreateMap<TaskChanges, TaskChangesDTO>();
            CreateMap<TaskChangesDTO, TaskChanges>();
        }
    }
}
