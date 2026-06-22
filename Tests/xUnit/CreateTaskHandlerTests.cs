using FluentAssertions;
using NSubstitute;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Application.DTOs.Tasks;
using Shared.Messages;
using Shared.Exceptions;

namespace xUnit
{
    public class CreateTaskHandlerTests
    {
        private readonly ITasksRepo _repoMock;
        private readonly IBus _busMock;
        private readonly ILogger<CreateTaskHandler> _loggerMock;
        private readonly CreateTaskHandler _handler;

        public CreateTaskHandlerTests()
        {
            _repoMock = Substitute.For<ITasksRepo>();
            _busMock = Substitute.For<IBus>();
            _loggerMock = Substitute.For<ILogger<CreateTaskHandler>>();
            _handler = new CreateTaskHandler(_repoMock, _busMock, _loggerMock);
        }

        //Tests of create task command handler

        [Fact]
        public async Task Handle_ValidCommand_ShouldCreateTaskPublishEventAndReturnId()
        {
            //Arrange
            var expectedTaskId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            var command = new CreateTaskCommand(new CreateTaskDTO
            {
                Name = "Test task",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = "Low",
                Status = "Backlog"
            });

            _repoMock.CreateAsync(Arg.Any<EmployeesTasksTracker.TasksTrackerService.Core.Models.Task>(), cancellationToken).Returns(expectedTaskId);

            //Act
            var result = await _handler.Handle(command, cancellationToken);

            //Assert
            result.Should().Be(expectedTaskId);

            await _repoMock.Received(1).CreateAsync(Arg.Is<EmployeesTasksTracker.TasksTrackerService.Core.Models.Task>(t =>
            t.Name == command.Task.Name && t.Priority.ToString() == command.Task.Priority), cancellationToken);

            await _busMock.Received(1).Publish(Arg.Is<TaskCreated>(m =>
            m.TaskId == expectedTaskId && m.Name == command.Task.Name && m.Priority == command.Task.Priority));

            _loggerMock.Received(1).Log(LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(v => v.ToString().Contains($"Successfully created new task with id {expectedTaskId}")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>());
        }

        [Fact]
        public async Task Handle_InvalidPriority_ShouldThrowDomainException()
        {
            //arrange
            var command = new CreateTaskCommand(new CreateTaskDTO
            {
                Name = "Invalid priority task",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = "SuperHigh",
                Status = "Backlog"
            });

            //Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            //Assert
            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("Unknown priority SuperHigh");

            await _repoMock.DidNotReceive().CreateAsync(Arg.Any<EmployeesTasksTracker.TasksTrackerService.Core.Models.Task>(), Arg.Any<CancellationToken>());

            await _busMock.DidNotReceive().Publish(Arg.Any<TaskCreated>());
        }

        [Fact]
        public async Task Handle_InvalidStatus_ShouldThrowDomainException()
        {
            //arrange
            var command = new CreateTaskCommand(new CreateTaskDTO
            {
                Name = "Invalid priority task",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = "Low",
                Status = "Invalid"
            });

            //Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            //Assert
            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("Unknown status Invalid");

            await _repoMock.DidNotReceive().CreateAsync(Arg.Any<EmployeesTasksTracker.TasksTrackerService.Core.Models.Task>(), Arg.Any<CancellationToken>());

            await _busMock.DidNotReceive().Publish(Arg.Any<TaskCreated>());
        }
    }
}
