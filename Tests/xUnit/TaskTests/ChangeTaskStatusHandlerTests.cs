using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shared.Exceptions;
using Shared.Messages;
using Task = EmployeesTasksTracker.TasksTrackerService.Core.Models.Task;

namespace xUnit.TaskTests
{
    public class ChangeTaskStatusHandlerTests
    {
        private readonly ITasksRepo _repoMock;
        private readonly IBus _busMock;
        private readonly HybridCache _cacheMock;
        private readonly ILogger<ChangeTaskStatusHandler> _loggerMock;
        private readonly ChangeTaskStatusHandler _handler;

        public ChangeTaskStatusHandlerTests()
        {
            _repoMock = Substitute.For<ITasksRepo>();
            _busMock = Substitute.For<IBus>();
            _cacheMock = Substitute.For<HybridCache>();
            _loggerMock = Substitute.For<ILogger<ChangeTaskStatusHandler>>();
            _handler = new ChangeTaskStatusHandler(_repoMock, _busMock, _cacheMock, _loggerMock);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ValidCommand_ShouldChangeTaskStatusAndPublishEvent()
        {
            //Arrange
            var taskId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
            var newStatus = "Current";

            var existingTask = new Task
            {
                Id = taskId,
                Name = "Test task",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.Low
            };

            var oldStatus = existingTask.Status.ToString();

            var command = new ChangeTaskStatusCommand(taskId, newStatus);

            var changes = new List<string>
                {
                    $"Status changed from {oldStatus} to {newStatus}"
                };

            _repoMock.GetByIdAsync(taskId, cancellationToken).Returns(existingTask);
            _repoMock.UpdateAsync(Arg.Any<Task>(), cancellationToken).Returns(existingTask);

            //Act
            await _handler.Handle(command, cancellationToken);

            //Assert
            await _repoMock.Received(1).UpdateAsync(Arg.Is<Task>(t =>
            t.Id == command.TaskId), cancellationToken);

            await _busMock.Received(1).Publish(Arg.Is<TaskDataChanged>(m =>
            m.Changes.SequenceEqual(changes)), cancellationToken);

            await _busMock.Received(1).Publish(Arg.Is<TaskStatusChanged>(m =>
            m.TaskId == command.TaskId 
            && m.OldStatus == oldStatus 
            && m.NewStatus == newStatus), cancellationToken);

            _loggerMock.Received(1).Log(LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(v => v.ToString().Contains($"Successfully changed status from {oldStatus} to {newStatus} for task: {existingTask.Name}")),
            Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>());
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_WrongNewStatus_ShouldThrowDomainException()
        {
            //Arrange
            var taskId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
            var newStatus = "Testing";

            var existingTask = new Task
            {
                Id = taskId,
                Name = "Test task",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.Low
            };

            var oldStatus = existingTask.Status;

            var command = new ChangeTaskStatusCommand(taskId, newStatus);

            _repoMock.GetByIdAsync(taskId, cancellationToken).Returns(existingTask);

            //Act
            Func<System.Threading.Tasks.Task> act = async ()=> await _handler.Handle(command, cancellationToken);

            //Assert
            await act.Should().ThrowAsync<DomainException>()
                .WithMessage($"Task can not change from {oldStatus} to {newStatus}!");
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_InvalidNewStatus_ShouldThrowArgumentNullException()
        {
            //Arrange
            var taskId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
            string newStatus = "InvalidStatus";

            var existingTask = new Task
            {
                Id = taskId,
                Name = "Test task",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.Low
            };

            var command = new ChangeTaskStatusCommand(taskId, newStatus);

            _repoMock.GetByIdAsync(taskId, cancellationToken).Returns(existingTask);

            //Act
            Func<System.Threading.Tasks.Task> act = async () => await _handler.Handle(command, cancellationToken);

            //Assert
            await act.Should().ThrowAsync<DomainException>()
                .WithMessage($"Unknown status {newStatus}");
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_NullNewStatus_ShouldThrowArgumentNullException()
        {
            //Arrange
            var taskId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
            string newStatus = null;

            var existingTask = new Task
            {
                Id = taskId,
                Name = "Test task",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.Low
            };

            var oldStatus = existingTask.Status;

            var command = new ChangeTaskStatusCommand(taskId, newStatus);

            _repoMock.GetByIdAsync(taskId, cancellationToken).Returns(existingTask);

            //Act
            Func<System.Threading.Tasks.Task> act = async () => await _handler.Handle(command, cancellationToken);

            //Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Given status was null!*")
                .WithParameterName("request");
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_SameStatusAsItIsAlreadyInTask_ShouldThrowDomainException()
        {
            //Arrange
            var taskId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
            var newStatus = "Backlog";

            var existingTask = new Task
            {
                Id = taskId,
                Name = "Test task",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.Low
            };

            var command = new ChangeTaskStatusCommand(taskId, newStatus);

            _repoMock.GetByIdAsync(taskId, cancellationToken).Returns(existingTask);

            //Act
            Func<System.Threading.Tasks.Task> act = async () => await _handler.Handle(command, cancellationToken);

            //Assert
            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("Task status has not changed, because it were the same as before");
        }
    }
}
