using Task = EmployeesTasksTracker.TasksTrackerService.Core.Models.Task;
using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shared.Exceptions;
using Shared.Messages;
using Shared.Interfaces;

namespace xUnit.TaskTests
{
    public class EditTaskHandlerTests
    {
        private readonly ITasksRepo _repoMock;
        private readonly ILogger<EditTaskHandler> _loggerMock;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly HybridCache _cacheMock;
        private readonly EditTaskHandler _handler;

        public EditTaskHandlerTests()
        {
            _repoMock = Substitute.For<ITasksRepo>();
            _cacheMock = Substitute.For<HybridCache>();
            _loggerMock = Substitute.For<ILogger<EditTaskHandler>>();
            _kafkaProducer = Substitute.For<IKafkaProducer>();
            _handler = new EditTaskHandler(_repoMock, _kafkaProducer, _cacheMock, _loggerMock);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ValidCommand_ShouldEditTaskPublishEventAndReturnDTO()
        {
            //Arrange
            var taskId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            var existingTask = new Task
            {
                Id = taskId,
                Name = "Test task",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.Low
            };

            var command = new EditTaskCommand(new EditTaskDTO
            {
                Id = taskId,
                Name = "Changed task",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = "Medium"
            });

            var expectedTaskDTO = new TaskDTO
            {
                Name = command.TaskToEdit.Name,
                Description = command.TaskToEdit.Description,
                CreatedAt = command.TaskToEdit.CreatedAt,
                Deadline = command.TaskToEdit.Deadline,
                Priority = command.TaskToEdit.Priority,
                Status = existingTask.Status.ToString()
            };

            List<string> changes = ["Name changed from Test task to Changed task", "Priority changed from Low to Medium"];

            _repoMock.GetByIdAsync(taskId, cancellationToken).Returns(existingTask);

            _repoMock.UpdateAsync(Arg.Any<Task>(), cancellationToken).Returns(existingTask);

            //Act
            var result = await _handler.Handle(command, cancellationToken);

            //Assert
            result.Should().Be(expectedTaskDTO);

            await _repoMock.Received(1).UpdateAsync(Arg.Is<Task>(t =>
            t.Id == command.TaskToEdit.Id && t.Priority.ToString() == command.TaskToEdit.Priority), cancellationToken);

            await _kafkaProducer.Received(1).PublishAsync(Arg.Is<TaskDataChanged>(m =>
            m.Changes.SequenceEqual(changes)));

            _loggerMock.Received(1).Log(LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(v => v.ToString().Contains($"Successfully edited task with id {taskId}")),
            Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>());
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_InvalidPriority_ShouldThrowDomainException()
        {
            //arrange
            var taskId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            var command = new EditTaskCommand(new EditTaskDTO
            {
                Id = taskId,
                Name = "Invalid priority task",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = "SuperHigh"
            });

            //Act
            Func<System.Threading.Tasks.Task> act = async() => await _handler.Handle(command, cancellationToken);

            //Assert
            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("Unknown priority SuperHigh");

            await _repoMock.DidNotReceive().UpdateAsync(Arg.Any<Task>(), Arg.Any<CancellationToken>());

            await _kafkaProducer.DidNotReceive().PublishAsync(Arg.Any<TaskDataChanged>());
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_InvalidTaskId_ShouldThrowNotFoundException()
        {
            //Arrange
            var taskId = Guid.NewGuid();
            var wrongId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            var existingTask = new Task
            {
                Id = taskId,
                Name = "Test task",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.Low
            };

            var command = new EditTaskCommand(new EditTaskDTO
            {
                Id = wrongId,
                Name = "Changed task",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = "Medium"
            });

            _repoMock.GetByIdAsync(taskId, cancellationToken).Returns(existingTask);

            //Act
            Func<System.Threading.Tasks.Task> act = async () => await _handler.Handle(command, cancellationToken);

            //Assert
            act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Entity task with key {wrongId} was not found!");

            await _repoMock.DidNotReceive().UpdateAsync(Arg.Any<Task>(), Arg.Any<CancellationToken>());

            await _kafkaProducer.DidNotReceive().PublishAsync(Arg.Any<TaskDataChanged>());
        }
    }
}
