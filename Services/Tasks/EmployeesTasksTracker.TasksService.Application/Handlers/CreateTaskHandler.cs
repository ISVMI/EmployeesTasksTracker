using AutoMapper;
using EmployeesTasksTracker.TasksService.Application.Commands;
using EmployeesTasksTracker.TasksService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksService.Application.Handlers
{
    public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, Guid>
    {
        private readonly ITasksRepo _repo;
        private readonly IMapper _mapper;

        public CreateTaskHandler(ITasksRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newTask = _mapper.Map<Core.Models.Task>(request.Task);
                await _repo.CreateAsync(newTask, cancellationToken);
                return newTask.Id;
            }
            catch (Exception ex) 
            {
                throw new Exception($"Could not create new task: {ex.Message}");
            }
        }
    }
}
