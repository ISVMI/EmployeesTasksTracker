using AutoMapper;
using EmployeesTasksTracker.HistoryService.Application.Commands;
using EmployeesTasksTracker.HistoryService.Core.Interfaces;
using EmployeesTasksTracker.HistoryService.Core.Models;
using MediatR;

namespace EmployeesTasksTracker.HistoryService.Application.Handlers
{
    public class CreateTaskChangesRecordHandler : IRequestHandler<CreateTaskChangesRecordCommand, Guid>
    {
        private readonly ITaskChangesRepo _repo;
        private readonly IMapper _mapper;

        public CreateTaskChangesRecordHandler(ITaskChangesRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateTaskChangesRecordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var taskChanges = _mapper.Map<TaskChanges>(request.TaskChanges);

                var taskChangesId = await _repo.CreateTaskChangesRecord(taskChanges, cancellationToken);

                return taskChangesId;
            }
            catch (Exception ex) 
            {
                var message = $"Could not create record of task changes {ex.Message}";

                Console.WriteLine(message);

                throw new Exception(message);

            }
        }
    }
}
