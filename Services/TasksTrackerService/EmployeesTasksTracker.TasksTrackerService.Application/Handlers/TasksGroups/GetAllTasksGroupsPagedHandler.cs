using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Shared.DTOs;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.TasksGroups
{
    public class GetAllTasksPagedHandler : IRequestHandler<GetAllTasksGroupsPagedQuery, PagedResponse<TasksGroupDTO>>
    {
        private readonly ITasksGroupsRepo _repo;

        public GetAllTasksPagedHandler(ITasksGroupsRepo repo)
        {
            _repo = repo;
        }

        public async Task<PagedResponse<TasksGroupDTO>> Handle(GetAllTasksGroupsPagedQuery request, CancellationToken token)
        {
            var (items, totalCount) = await _repo.GetPagedAsync(request.Page, request.PageSize, token);

            var dtoList = new List<TasksGroupDTO>();

            foreach (var item in items)
            {
                dtoList.Add(new TasksGroupDTO { Name = item.Name });
            }

            return new PagedResponse<TasksGroupDTO>(dtoList, totalCount, request.Page, request.PageSize);
        }
    }
}
