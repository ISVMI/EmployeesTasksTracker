using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Shared.DTOs;
using Shared.Methods;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks
{
    internal class GetAllTasksPagedHandler : IRequestHandler<GetAllTasksPagedQuery, PagedResponse<TaskDTO>>
    {
        private readonly ITasksRepo _repo;

        public GetAllTasksPagedHandler(ITasksRepo repo)
        {
            _repo = repo;
        }

        public async Task<PagedResponse<TaskDTO>> Handle(GetAllTasksPagedQuery request, CancellationToken token)
        {
            var (items, totalCount) = await _repo.GetPagedAsync(request.Page, request.PageSize, token);

            var dtoList = new List<TaskDTO>();

            foreach (var item in items)
            {
                dtoList.Add(new TaskDTO
                {
                    Name = item.Name,
                    Description = item.Description,
                    Deadline = item.Deadline,
                    CreatedAt = item.CreatedAt,
                    Status = EnumsHumanizer.Translate(item.Status.ToString()),
                    Priority = EnumsHumanizer.Translate(item.Priority.ToString())
                });
            }

            return new PagedResponse<TaskDTO>(dtoList, totalCount, request.Page, request.PageSize);
        }
    }
}
