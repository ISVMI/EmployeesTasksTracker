using AutoMapper;
using EmployeesTasksTracker.HistoryService.Application.DTOs;
using EmployeesTasksTracker.HistoryService.Application.Queries;
using EmployeesTasksTracker.HistoryService.Core.Interfaces;
using MediatR;

namespace EmployeesTasksTracker.HistoryService.Application.Handlers
{
    internal class GetAllTasksChangesHandler : IRequestHandler<GetAllTasksChangesQuery, IEnumerable<TaskChangesDTO>>
    {
        private readonly ITaskChangesRepo _repo;
        private readonly IMapper _mapper;

        public GetAllTasksChangesHandler(ITaskChangesRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskChangesDTO>> Handle(GetAllTasksChangesQuery request, CancellationToken cancellationToken)
        {
                var taskChanges = await _repo.GetAllChanges(cancellationToken);

                var result = _mapper.Map<IEnumerable<TaskChangesDTO>>(taskChanges);

                return result;
        }
    }
}
