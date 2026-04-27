using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Projects;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using MediatR;
using Shared.Exceptions;

namespace EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Projects
{
    public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, bool>
    {
        private readonly IProjectsRepo _projectsRepo;
        private readonly ITasksRepo _tasksRepo;

        public DeleteProjectHandler(IProjectsRepo projectsRepo, ITasksRepo tasksRepo)
        {
            _projectsRepo = projectsRepo;
            _tasksRepo = tasksRepo;
        }

        public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var relatedTasks = await _tasksRepo.GetAllFilteredAsync(default, default, request.Id, cancellationToken);

            if (relatedTasks.Any())
            {
                foreach (var task in relatedTasks)
                {
                    if (task.Status != Core.Enums.Status.Canceled || task.Status != Core.Enums.Status.Completed)
                    {
                        throw new DomainException($"Couldn't delete project! Task {task.Name} is {task.Status}!");
                    }
                }
            }

            return await _projectsRepo.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
