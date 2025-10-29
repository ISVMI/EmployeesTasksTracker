using AutoMapper;
using EmployeesTasksTracker.TasksGroupsService.Application.Commands;
using EmployeesTasksTracker.TasksGroupsService.Core.Interfaces;
using EmployeesTasksTracker.TasksGroupsService.Core.Models;
using MediatR;

namespace EmployeesTasksTracker.TasksGroupsService.Application.Handlers
{
    public class CreateTasksGroupHandler : IRequestHandler<CreateTasksGroupCommand, Guid>
    {
        private readonly ITasksGroupsRepo _repo;
        private readonly IMapper _mapper;

        public CreateTasksGroupHandler(ITasksGroupsRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateTasksGroupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newTasksGroup = _mapper.Map<TasksGroup>(request.TasksGroup);
                await _repo.CreateAsync(newTasksGroup, cancellationToken);
                return newTasksGroup.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
