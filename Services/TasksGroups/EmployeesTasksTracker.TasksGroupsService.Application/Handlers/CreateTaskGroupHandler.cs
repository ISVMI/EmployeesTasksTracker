using AutoMapper;
using EmployeesTasksTracker.TasksGroupsService.Application.Commands;
using EmployeesTasksTracker.TasksGroupsService.Core.Interfaces;
using EmployeesTasksTracker.TasksGroupsService.Core.Models;
using MediatR;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Handlers
{
    public class CreateTaskGroupHandler : IRequestHandler<CreateTaskGroupCommand, Guid>
    {
        private readonly ITaskGroupsRepo _repo;
        private readonly IMapper _mapper;

        public CreateTaskGroupHandler(ITaskGroupsRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateTaskGroupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newTaskGroup = _mapper.Map<TaskGroup>(request.TaskGroup);
                await _repo.CreateAsync(newTaskGroup, cancellationToken);
                return newTaskGroup.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create new task group: {ex.Message}");

                return Guid.Empty;
            }
        }
    }
}
