using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.TasksGroups
{
    public class GetAllTasksGroupsHandler : IRequestHandler<GetAllTasksGroupsQuery, IEnumerable<TasksGroupDTO>>
    {
        private readonly ITasksGroupsRepo _repo;

        public GetAllTasksGroupsHandler(ITasksGroupsRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<TasksGroupDTO>> Handle(GetAllTasksGroupsQuery request, CancellationToken cancellationToken)
        {
            var tasksGroups = await _repo.GetAllAsync(cancellationToken);

            var tasksGroupsDtoList = new List<TasksGroupDTO>();

            foreach (var taskGroup in tasksGroups) 
            {
                tasksGroupsDtoList.Add(new TasksGroupDTO
                {
                    Name = taskGroup.Name
                });
            }

            return tasksGroupsDtoList;
        }
    }
}
