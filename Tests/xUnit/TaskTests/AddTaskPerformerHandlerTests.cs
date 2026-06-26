using EmployeesTasksTracker.TasksTrackerService.Application.Commands.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Application.Handlers.Tasks;
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
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
    public class AddTaskPerformerHandlerTests
    {
        private readonly ITasksRepo _repoMock;
        private readonly IBus _busMock;
        private readonly ITaskEmployeeRepo _taskEmployeeRepoMock;
        private readonly HybridCache _cacheMock;
        private readonly ILogger<AddTaskPerformerHandler> _loggerMock;
        private readonly AddTaskPerformerHandler _handler;

        public AddTaskPerformerHandlerTests()
        {
            _repoMock = Substitute.For<ITasksRepo>();
            _busMock = Substitute.For<IBus>();
            _taskEmployeeRepoMock = Substitute.For<ITaskEmployeeRepo>();
            _cacheMock = Substitute.For<HybridCache>();
            _loggerMock = Substitute.For<ILogger<AddTaskPerformerHandler>>();
            _handler = new AddTaskPerformerHandler(_repoMock, _taskEmployeeRepoMock, _busMock, _cacheMock, _loggerMock);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ValidCommand_ShouldAddPerformerToTaskAndPublishMessages()
        {
            //Arrange
            var taskId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            var task = new Task
            {
                Id = taskId,
                Name = "Test task",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.Low
            };

            var employeeTask = new List<TaskEmployee>();

            var changes = new List<string>
                {
                    $"Added performer with id: {employeeId}"
                };

            var command = new AddTaskPerformerCommand(employeeId, taskId);

            _repoMock.GetByIdAsync(taskId, cancellationToken).Returns(task);
            _taskEmployeeRepoMock.GetAllById(taskId, employeeId, cancellationToken).Returns(employeeTask);

            //Act
            await _handler.Handle(command, cancellationToken);

            //Assert

            await _busMock.Received(1).Publish(Arg.Is<TaskDataChanged>(m =>
            m.Changes.SequenceEqual(changes)), cancellationToken);

            await _busMock.Received(1).Publish(Arg.Is<EmployeeAssigned>(m =>
            m.TaskId == taskId && m.EmployeeId == employeeId), cancellationToken);

            _loggerMock.Received(1).Log(LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(v => v.ToString().Contains($"Successfully added performer with id {employeeId} for task {taskId}")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>());
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_AlreadyAssignedAsPerformer_ShouldThrowDomainException()
        {
            //Arrange
            var taskId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            var task = new Task
            {
                Id = taskId,
                Name = "Test task",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.Low
            };

            var firstTaskEmployee = new TaskEmployee
            {
                EmployeeId = employeeId,
                TaskId = taskId,
                EmployeeRoleInTask = RoleInTask.Performer
            };

            var employeeTask = new List<TaskEmployee> { firstTaskEmployee };

            var command = new AddTaskPerformerCommand(employeeId, taskId);

            _repoMock.GetByIdAsync(taskId, cancellationToken).Returns(task);
            _taskEmployeeRepoMock.GetAllById(taskId, employeeId, cancellationToken).Returns(employeeTask);

            //Act
            Func<System.Threading.Tasks.Task> act = async () => await _handler.Handle(command, cancellationToken);

            //Assert
            await act.Should().ThrowAsync<DomainException>()
                .WithMessage($"Employee with id: {employeeId} already assigned as {firstTaskEmployee.EmployeeRoleInTask}!");
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_AlreadyAssignedAsObserver_ShouldThrowDomainException()
        {
            //Arrange
            var taskId = Guid.NewGuid();
            var employeeId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            var task = new Task
            {
                Id = taskId,
                Name = "Test task",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.Low
            };

            var firstTaskEmployee = new TaskEmployee
            {
                EmployeeId = employeeId,
                TaskId = taskId,
                EmployeeRoleInTask = RoleInTask.Observer
            };

            var employeeTask = new List<TaskEmployee> { firstTaskEmployee };

            var command = new AddTaskPerformerCommand(employeeId, taskId);

            _repoMock.GetByIdAsync(taskId, cancellationToken).Returns(task);
            _taskEmployeeRepoMock.GetAllById(taskId, employeeId, cancellationToken).Returns(employeeTask);

            //Act
            Func<System.Threading.Tasks.Task> act = async () => await _handler.Handle(command, cancellationToken);

            //Assert
            await act.Should().ThrowAsync<DomainException>()
                .WithMessage($"Employee with id: {employeeId} already assigned as {firstTaskEmployee.EmployeeRoleInTask}!");
        }
    }
}