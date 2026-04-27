using EmployeesTasksTracker.TasksTrackerService.Application.Commands.TasksGroups;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.TasksGroups
{
    public class DeleteTasksGroupHandler : IRequestHandler<DeleteTasksGroupCommand, bool>
    {
        private readonly ITasksGroupsRepo _tasksGroupsRepo;
        private readonly ITasksRepo _tasksRepo;

        public DeleteTasksGroupHandler(ITasksGroupsRepo tasksGroupsRepo, ITasksRepo tasksRepo)
        {
            _tasksGroupsRepo = tasksGroupsRepo;
            _tasksRepo = tasksRepo;
        }

        public async Task<bool> Handle(DeleteTasksGroupCommand request, CancellationToken cancellationToken)
        {
            var relatedTasks = await _tasksRepo.GetAllFilteredAsync(default, request.Id, default, cancellationToken);

            if (relatedTasks.Any())
            {
                foreach (var task in relatedTasks)
                {
                    if (task.Status != Core.Enums.Status.Canceled || task.Status != Core.Enums.Status.Completed) 
                    {
                        throw new DomainException($"Couldn't delete tasks group! Task {task.Name} is {task.Status}!");
                    }
                }
            }

            return await _tasksGroupsRepo.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
