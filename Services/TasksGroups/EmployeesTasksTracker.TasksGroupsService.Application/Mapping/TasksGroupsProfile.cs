using AutoMapper;
using EmployeesTasksTracker.TasksGroupsService.Application.DTOs;
using EmployeesTasksTracker.TasksGroupsService.Core.Models;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Mapping
{
    public class TasksGroupsProfile : Profile
    {
        public TasksGroupsProfile()
        {
            CreateMap<TasksGroup, TasksGroupDTO>();
            CreateMap<EditTasksGroupDTO, TasksGroup>();
        }
    }
}
