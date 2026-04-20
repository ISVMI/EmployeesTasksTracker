using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Employees;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.Employees;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Shared.DTOs;
using Shared.Methods;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Employees
{
    internal class GetAllTasksPagedHandler : IRequestHandler<GetAllEmployeesPagedQuery, PagedResponse<EmployeeDTO>>
    {
        private readonly IEmployeesRepo _repo;

        public GetAllTasksPagedHandler(IEmployeesRepo repo)
        {
            _repo = repo;
        }

        public async Task<PagedResponse<EmployeeDTO>> Handle(GetAllEmployeesPagedQuery request, CancellationToken token)
        {
            var (items, totalCount) = await _repo.GetPagedAsync(request.Page, request.PageSize, token);

            var dtoList = new List<EmployeeDTO>();

            foreach (var item in items)
            {
                dtoList.Add(new EmployeeDTO
                {
                    Name = item.Name,
                    Surname = item.Surname,
                    Patronymic = item.Patronymic,
                    UserName = item.UserName,
                    Role = EnumsHumanizer.Translate(item.Role.ToString())
                });
            }

            return new PagedResponse<EmployeeDTO>(dtoList, totalCount, request.Page, request.PageSize);
        }
    }
}
