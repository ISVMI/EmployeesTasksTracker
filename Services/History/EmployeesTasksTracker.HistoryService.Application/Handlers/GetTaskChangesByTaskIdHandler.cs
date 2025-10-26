using AutoMapper;
using EmployeesTasksTracker.HistoryService.Application.DTOs;
using EmployeesTasksTracker.HistoryService.Application.Queries;
using EmployeesTasksTracker.HistoryService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.HistoryService.Application.Handlers
{
    public class GetTaskChangesByTaskIdHandler : IRequestHandler<GetTaskChangesByTaskIdQuery, IEnumerable<TaskChangesDTO>>
    {
        private readonly ITaskChangesRepo _repo;
        private readonly IMapper _mapper;

        public GetTaskChangesByTaskIdHandler(ITaskChangesRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskChangesDTO>> Handle(GetTaskChangesByTaskIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var taskChanges = await _repo.GetChangesByTaskId(request.TaskId, cancellationToken);

                var result = _mapper.Map<IEnumerable<TaskChangesDTO>>(taskChanges);

                return result;
            }
            catch (Exception ex) 
            {
                var message = $"Could not get changes by task id {ex.Message}";

                Console.WriteLine(message);

                throw new Exception(message);
            }
        }
    }
}
