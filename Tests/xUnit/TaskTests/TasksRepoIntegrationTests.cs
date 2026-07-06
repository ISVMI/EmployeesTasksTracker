using Bogus.DataSets;
using EmployeesTasksTracker.TasksTrackerService.Core.Enums;
using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using EmployeesTasksTracker.TasksTrackerService.Infrastructure.Data;
using EmployeesTasksTracker.TasksTrackerService.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Xunit;
using Task = EmployeesTasksTracker.TasksTrackerService.Core.Models.Task;

namespace xUnit.TaskTests
{
    public class TasksRepoIntegrationTests
    {
        private readonly SqliteConnection _connection;
        private readonly TasksTrackerContext _context;

        public TasksRepoIntegrationTests()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<TasksTrackerContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new TasksTrackerContext(options);
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllFilteredAsync_WhenFilteredByProjectId_ShouldReturnOnlyMatchingTasks()
        {
            //Arrange
            var cancellationToken = CancellationToken.None;

            var targetProject = new Project
            {
                Name = "Target project",
                Description = "Description"
            };

            var otherProject = new Project
            {
                Name = "Other project",
                Description = "Description"
            };

            await _context.Projects.AddRangeAsync(targetProject, otherProject);

            var tasksGroup = new TasksGroup
            {
                Name = "Tasks group"
            };

            await _context.TasksGroups.AddAsync(tasksGroup, cancellationToken);

            var task1 = new Task
            {
                Name = "Target project task1",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.Low,
                ProjectId = targetProject.Id,
                TasksGroupId = tasksGroup.Id
            };

            var task2 = new Task
            {
                Name = "Target project task2",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.Medium,
                ProjectId = targetProject.Id,
                TasksGroupId = tasksGroup.Id
            };

            var task3 = new Task
            {
                Name = "Other project task",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.High,
                ProjectId = otherProject.Id,
                TasksGroupId = tasksGroup.Id
            };

            await _context.Tasks.AddRangeAsync(task1, task2, task3);
            await _context.SaveChangesAsync(cancellationToken);

            var repo = new TasksRepo(_context);

            //Act
            var result = await repo.GetAllFilteredAsync(null, null, targetProject.Id, cancellationToken);

            //Assert
            result.Should().HaveCount(2);

            result.Should().ContainSingle(t => t.Id == task1.Id);
            result.Should().ContainSingle(t => t.Id == task2.Id);

            result.Should().NotContain(t => t.Id == task3.Id);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllFilteredAsync_WhenFilteredByTasksGroupId_ShouldReturnOnlyMatchingTasks()
        {
            //Arrange
            var cancellationToken = CancellationToken.None;

            var project = new Project
            {
                Name = "Other project",
                Description = "Description"
            };

            await _context.Projects.AddAsync(project, cancellationToken);

            var targetTasksGroup = new TasksGroup
            {
                Name = "Target tasks group"
            };

            var otherTasksGroup = new TasksGroup
            {
                Name = "Other tasks group"
            };

            await _context.TasksGroups.AddRangeAsync(targetTasksGroup, otherTasksGroup);

            var task1 = new Task
            {
                Name = "Target tasks group task1",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.Low,
                ProjectId = project.Id,
                TasksGroupId = targetTasksGroup.Id
            };

            var task2 = new Task
            {
                Name = "Target tasks group task2",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.Medium,
                ProjectId = project.Id,
                TasksGroupId = targetTasksGroup.Id
            };

            var task3 = new Task
            {
                Name = "Other tasks group task",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.High,
                ProjectId = project.Id,
                TasksGroupId = otherTasksGroup.Id
            };

            await _context.Tasks.AddRangeAsync(task1, task2, task3);
            await _context.SaveChangesAsync(cancellationToken);

            var repo = new TasksRepo(_context);

            //Act
            var result = await repo.GetAllFilteredAsync(null, targetTasksGroup.Id, null, cancellationToken);

            //Assert
            result.Should().HaveCount(2);

            result.Should().ContainSingle(t => t.Id == task1.Id);
            result.Should().ContainSingle(t => t.Id == task2.Id);

            result.Should().NotContain(t => t.Id == task3.Id);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllFilteredAsync_WhenFilteredByEmployeeId_ShouldReturnOnlyMatchingTasks()
        {
            //Arrange
            var cancellationToken = CancellationToken.None;

            var project = new Project
            {
                Name = "Target project",
                Description = "Description"
            };

            await _context.Projects.AddAsync(project, cancellationToken);

            var tasksGroup = new TasksGroup
            {
                Name = "Tasks group"
            };

            await _context.TasksGroups.AddAsync(tasksGroup, cancellationToken);

            var employee1 = new Employee
            {
                Name = "Name1",
                Surname = "Surname1",
                Patronymic = "Patronymic1",
                UserName = "UserName1",
                Role = EmployeeRole.QA
            };

            var employee2 = new Employee
            {
                Name = "Name2",
                Surname = "Surname2",
                Patronymic = "Patronymic2",
                UserName = "UserName2",
                Role = EmployeeRole.Developer
            };

            await _context.Employees.AddRangeAsync(employee1, employee2);

            var task1 = new Task
            {
                Name = "Target employee task1",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.Low,
                ProjectId = project.Id,
                TasksGroupId = tasksGroup.Id
            };

            var task2 = new Task
            {
                Name = "Target employee task2",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.Medium,
                ProjectId = project.Id,
                TasksGroupId = tasksGroup.Id
            };

            var task3 = new Task
            {
                Name = "Other employee task",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.High,
                ProjectId = project.Id,
                TasksGroupId = tasksGroup.Id
            };

            await _context.Tasks.AddRangeAsync(task1, task2, task3);

            var taskEmployee1 = new TaskEmployee
            {
                TaskId = task1.Id,
                EmployeeId = employee1.Id
            };

            var taskEmployee2 = new TaskEmployee
            {
                TaskId = task2.Id,
                EmployeeId = employee1.Id
            };

            var taskEmployee3 = new TaskEmployee
            {
                TaskId = task2.Id,
                EmployeeId = employee2.Id
            };

            await _context.TaskEmployees.AddRangeAsync(taskEmployee1, taskEmployee2, taskEmployee3);
            await _context.SaveChangesAsync(cancellationToken);

            var repo = new TasksRepo(_context);

            //Act
            var result = await repo.GetAllFilteredAsync(employee1.Id, null, null, cancellationToken);

            //Assert
            result.Should().HaveCount(2);

            result.Should().ContainSingle(t => t.Id == task1.Id);
            result.Should().ContainSingle(t => t.Id == task2.Id);

            result.Should().NotContain(t => t.Id == task3.Id);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllFilteredAsync_WhenFilteredByProjectIdAndTasksGroupId_ShouldReturnOnlyMatchingTasks()
        {
            //Arrange
            var cancellationToken = CancellationToken.None;

            var targetProject = new Project
            {
                Name = "Target project",
                Description = "Description"
            };

            var otherProject = new Project
            {
                Name = "Other project",
                Description = "Description"
            };

            await _context.Projects.AddRangeAsync(targetProject, otherProject);

            var targetTasksGroup = new TasksGroup
            {
                Name = "Target tasks group"
            };

            var otherTasksGroup = new TasksGroup
            {
                Name = "Other tasks group"
            };

            await _context.TasksGroups.AddRangeAsync(targetTasksGroup, otherTasksGroup);

            var task1 = new Task
            {
                Name = "Target task1",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.Low,
                ProjectId = targetProject.Id,
                TasksGroupId = targetTasksGroup.Id
            };

            var task2 = new Task
            {
                Name = "Target task2",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.Medium,
                ProjectId = targetProject.Id,
                TasksGroupId = targetTasksGroup.Id
            };

            var task3 = new Task
            {
                Name = "Other task",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.High,
                ProjectId = otherProject.Id,
                TasksGroupId = otherTasksGroup.Id
            };

            await _context.Tasks.AddRangeAsync(task1, task2, task3);
            await _context.SaveChangesAsync(cancellationToken);

            var repo = new TasksRepo(_context);

            //Act
            var result = await repo.GetAllFilteredAsync(null, targetTasksGroup.Id, targetProject.Id, cancellationToken);

            //Assert
            result.Should().HaveCount(2);

            result.Should().ContainSingle(t => t.Id == task1.Id);
            result.Should().ContainSingle(t => t.Id == task2.Id);

            result.Should().NotContain(t => t.Id == task3.Id);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllFilteredAsync_WhenFilteredByAllIds_ShouldReturnOnlyMatchingTasks()
        {
            //Arrange
            var cancellationToken = CancellationToken.None;

            var targetProject = new Project
            {
                Name = "Target project",
                Description = "Description"
            };

            var otherProject = new Project
            {
                Name = "Other project",
                Description = "Description"
            };

            await _context.Projects.AddRangeAsync(targetProject, otherProject);

            var targetTasksGroup = new TasksGroup
            {
                Name = "Target tasks group"
            };

            var otherTasksGroup = new TasksGroup
            {
                Name = "Other tasks group"
            };

            await _context.TasksGroups.AddRangeAsync(targetTasksGroup, otherTasksGroup);

            var targetEmployee = new Employee
            {
                Name = "Name1",
                Surname = "Surname1",
                Patronymic = "Patronymic1",
                UserName = "UserName1",
                Role = EmployeeRole.QA
            };

            var otherEmployee = new Employee
            {
                Name = "Name2",
                Surname = "Surname2",
                Patronymic = "Patronymic2",
                UserName = "UserName2",
                Role = EmployeeRole.Developer
            };

            await _context.Employees.AddRangeAsync(targetEmployee, otherEmployee);

            var task1 = new Task
            {
                Name = "Target task1",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.Low,
                ProjectId = targetProject.Id,
                TasksGroupId = targetTasksGroup.Id
            };

            var task2 = new Task
            {
                Name = "Target task2",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.Medium,
                ProjectId = targetProject.Id,
                TasksGroupId = targetTasksGroup.Id
            };

            var task3 = new Task
            {
                Name = "Other task",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.High,
                ProjectId = otherProject.Id,
                TasksGroupId = otherTasksGroup.Id
            };

            await _context.Tasks.AddRangeAsync(task1, task2, task3);

            var taskEmployee1 = new TaskEmployee
            {
                TaskId = task1.Id,
                EmployeeId = targetEmployee.Id
            };

            var taskEmployee2 = new TaskEmployee
            {
                TaskId = task2.Id,
                EmployeeId = targetEmployee.Id
            };

            var taskEmployee3 = new TaskEmployee
            {
                TaskId = task3.Id,
                EmployeeId = otherEmployee.Id
            };

            await _context.TaskEmployees.AddRangeAsync(taskEmployee1, taskEmployee2, taskEmployee3);
            await _context.SaveChangesAsync(cancellationToken);

            var repo = new TasksRepo(_context);

            //Act
            var result = await repo.GetAllFilteredAsync(targetEmployee.Id, targetTasksGroup.Id, targetProject.Id, cancellationToken);

            //Assert
            result.Should().HaveCount(2);

            result.Should().ContainSingle(t => t.Id == task1.Id);
            result.Should().ContainSingle(t => t.Id == task2.Id);

            result.Should().NotContain(t => t.Id == task3.Id);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllFilteredAsync_WhenFilteredByNullIds_ShouldReturnAllTasks()
        {
            //Arrange
            var cancellationToken = CancellationToken.None;

            var project = new Project
            {
                Name = "Project",
                Description = "Description"
            };

            await _context.Projects.AddAsync(project, cancellationToken);

            var tasksGroup = new TasksGroup
            {
                Name = "Tasks group"
            };

            await _context.TasksGroups.AddAsync(tasksGroup, cancellationToken);

            var task1 = new Task
            {
                Name = "Task1",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.Low,
                ProjectId = project.Id,
                TasksGroupId = tasksGroup.Id
            };

            var task2 = new Task
            {
                Name = "Task2",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.Medium,
                ProjectId = project.Id,
                TasksGroupId = tasksGroup.Id
            };

            var task3 = new Task
            {
                Name = "Task3",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow + TimeSpan.FromDays(60),
                Priority = Priority.High,
                ProjectId = project.Id,
                TasksGroupId = tasksGroup.Id
            };

            await _context.Tasks.AddRangeAsync(task1, task2, task3);
            await _context.SaveChangesAsync(cancellationToken);

            var repo = new TasksRepo(_context);

            //Act
            var result = await repo.GetAllFilteredAsync(null, null, null, cancellationToken);

            //Assert
            result.Should().HaveCount(3);

            result.Should().ContainSingle(t => t.Id == task1.Id);
            result.Should().ContainSingle(t => t.Id == task2.Id);
            result.Should().ContainSingle(t => t.Id == task3.Id);
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Dispose();
        }
    }
}
