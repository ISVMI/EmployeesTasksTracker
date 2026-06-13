using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.DTOs;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.TasksGroups
{
    public class GetAllTasksGroupsPagedHandler : IRequestHandler<GetAllTasksGroupsPagedQuery, PagedResponse<TasksGroupDTO>>
    {
        private readonly ITasksGroupsRepo _repo;
        private readonly ILogger<GetAllTasksGroupsPagedHandler> _logger;

        public GetAllTasksGroupsPagedHandler(ITasksGroupsRepo repo, ILogger<GetAllTasksGroupsPagedHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<PagedResponse<TasksGroupDTO>> Handle(GetAllTasksGroupsPagedQuery request, CancellationToken token)
        {
            var (items, totalCount) = await _repo.GetPagedAsync(request.Page, request.PageSize, token);

            var dtoList = new List<TasksGroupDTO>();

            foreach (var item in items)
            {
                dtoList.Add(new TasksGroupDTO { Name = item.Name });
            }

            _logger.LogInformation("Successfully {totalCount} tasks groups", totalCount);

            return new PagedResponse<TasksGroupDTO>(dtoList, totalCount, request.Page, request.PageSize);
        }
    }
}
