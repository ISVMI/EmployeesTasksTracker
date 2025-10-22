using AutoMapper;
using EmployeesTasksTracker.TasksService.Application.DTOs;
using EmployeesTasksTracker.TasksService.Core.Enums;

namespace EmployeesTasksTracker.TasksService.Application.Mapping
{
    public class TasksProfile : Profile
    {
        public TasksProfile()
        {
            CreateMap<Core.Models.Task, TaskDTO>();
            CreateMap<TaskDTO, Core.Models.Task>();
            CreateMap<EditTaskDTO, Core.Models.Task>();
            CreateMap<CreateTaskDTO, Core.Models.Task>();
            CreateMap<Status, string>();
            CreateMap<string, Status>();
        }
    }
}
