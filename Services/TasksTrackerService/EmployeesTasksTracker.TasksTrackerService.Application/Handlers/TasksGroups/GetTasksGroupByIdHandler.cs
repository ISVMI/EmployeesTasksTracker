using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Application.Queries.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.TasksGroups
{
    public class GetTasksGroupByIdHandler : IRequestHandler<GetTasksGroupByIdQuery, TasksGroupDTO>
    {
        private readonly ITasksGroupsRepo _repo;

        public GetTasksGroupByIdHandler(ITasksGroupsRepo repo)
        {
            _repo = repo;
        }

        public async Task<TasksGroupDTO> Handle(GetTasksGroupByIdQuery request, CancellationToken cancellationToken)
        {
                var tasksGroup = await _repo.GetByIdAsync(request.Id, cancellationToken);

                return new TasksGroupDTO
                {
                    Name = tasksGroup.Name
                };
        }
    }
}
