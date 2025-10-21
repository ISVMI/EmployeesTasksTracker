using AutoMapper;
using EmployeesTasksTracker.ProjectsService.Application.DTOs;
using EmployeesTasksTracker.ProjectsService.Core.Models;

namespace EmployeesTasksTracker.ProjectsService.Application.Mapping
{
    public class ProjectsProfile : Profile
    {
        public ProjectsProfile()
        {
            CreateMap<Project, ProjectDTO>();
            CreateMap<ProjectDTO, Project>();
            CreateMap<EditProjectDTO, Project>();
        }
    }
}
