using AutoMapper;
using EmployeesTasksTracker.TasksGroupsService.Application.DTOs;
using EmployeesTasksTracker.TasksGroupsService.Core.Models;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Mapping
{
    public class TaskGroupsProfile : Profile
    {
        public TaskGroupsProfile()
        {
            CreateMap<TaskGroup, TaskGroupDto>();
            CreateMap<EditTaskGroupDTO, TaskGroup>();
        }
    }
}
