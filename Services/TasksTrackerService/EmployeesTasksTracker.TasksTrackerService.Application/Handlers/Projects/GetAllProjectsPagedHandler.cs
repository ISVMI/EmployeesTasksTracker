using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Projects;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Projects;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.DTOs;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Projects
{
    internal class GetAllProjectsPagedHandler : IRequestHandler<GetAllProjectsPagedQuery, PagedResponse<ProjectDTO>>
    {
        private readonly IProjectsRepo _repo;
        private readonly ILogger<GetAllProjectsPagedHandler> _logger;

        public GetAllProjectsPagedHandler(IProjectsRepo repo, ILogger<GetAllProjectsPagedHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<PagedResponse<ProjectDTO>> Handle(GetAllProjectsPagedQuery request, CancellationToken token)
        {
            var (items, totalCount) = await _repo.GetPagedAsync(request.Page, request.PageSize, token);

            var dtoList = new List<ProjectDTO>();

            foreach (var item in items)
            {
                dtoList.Add(new ProjectDTO
                {
                    Name = item.Name,
                    Description = item.Description
                });
            }

            _logger.LogInformation("Successfully got {totalCount} projects", totalCount);

            return new PagedResponse<ProjectDTO>(dtoList, totalCount, request.Page, request.PageSize);
        }
    }
}
