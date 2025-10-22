using AutoMapper;
using EmployeesTasksTracker.TasksService.Application.DTOs;
using EmployeesTasksTracker.TasksService.Application.Queries;
using EmployeesTasksTracker.TasksService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.TasksService.Application.Handlers
{
    public class GetTaskByIdHandler : IRequestHandler<GetTaskByIdQuery, TaskDTO>
    {
        private readonly ITasksRepo _repo;
        private readonly IMapper _mapper;

        public GetTaskByIdHandler(ITasksRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<TaskDTO> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _repo.GetByIdAsync(request.Id, cancellationToken);

                return _mapper.Map<TaskDTO>(task);
            }
            catch (Exception ex) 
            {
                throw new Exception($"Could not get task : {ex.Message}");
            }
            
        }
    }
}
