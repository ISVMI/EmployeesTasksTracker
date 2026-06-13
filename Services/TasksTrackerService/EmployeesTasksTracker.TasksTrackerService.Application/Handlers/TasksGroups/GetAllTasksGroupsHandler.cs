using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.TasksGroups
{
    public class GetAllTasksGroupsHandler : IRequestHandler<GetAllTasksGroupsQuery, IEnumerable<TasksGroupDTO>>
    {
        private readonly ITasksGroupsRepo _repo;
        private readonly ILogger<GetAllTasksGroupsHandler> _logger;

        public GetAllTasksGroupsHandler(ITasksGroupsRepo repo, ILogger<GetAllTasksGroupsHandler> logger)
        {
            _repo = repo;
            _logger = logger;
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

            _logger.LogInformation("Successfully found tasks group {tasksGroupName}", tasksGroups.Count());

            return tasksGroupsDtoList;
        }
    }
}
