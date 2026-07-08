using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shared.Exceptions;
using Shared.Messages;
namespace Testing.TaskTests
{
    public class DeleteTaskHandlerTests
    {
        private readonly ITasksRepo _repoMock;
        private readonly IBus _busMock;
        private readonly HybridCache _cacheMock;
        private readonly ILogger<DeleteTaskHandler> _loggerMock;
        private readonly DeleteTaskHandler _handler;

        public DeleteTaskHandlerTests()
        {
            _repoMock = Substitute.For<ITasksRepo>();
            _busMock = Substitute.For<IBus>();
            _cacheMock = Substitute.For<HybridCache>();
            _loggerMock = Substitute.For<ILogger<DeleteTaskHandler>>();
            _handler = new DeleteTaskHandler(_repoMock, _busMock, _cacheMock, _loggerMock);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_InvalidTaskId_ShouldThrowNotFoundException()
        {
            //Arrange
            var wrongId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            var command = new DeleteTaskCommand(wrongId);

            _repoMock.DeleteAsync(wrongId, cancellationToken).Throws(new NotFoundException("task", wrongId));

            //Act
            Func<System.Threading.Tasks.Task> act = async () => await _handler.Handle(command, cancellationToken);

            //Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Entity task with key {wrongId} was not found!");

            await _busMock.DidNotReceive().Publish(Arg.Any<TaskDeleted>(), cancellationToken);
        }
    }
}
