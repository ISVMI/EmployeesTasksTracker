using EmployeesTasksTracker.TasksTrackerService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesTasksTracker.TasksTrackerService.Infrastructure.Data
{
    public class TasksTrackerContext : DbContext
    {
        public TasksTrackerContext(DbContextOptions<TasksTrackerContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Core.Models.Task> Tasks { get; set; }
        public DbSet<TasksGroup> TasksGroups { get; set; }
        public DbSet<TaskEmployee> TaskEmployees { get; set; }
        public DbSet<ProjectEmployee> ProjectEmployees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TaskEmployee>()
                .HasKey(te => new { te.TaskId, te.EmployeeId, te.EmployeeRoleInTask });

            modelBuilder.Entity<TaskEmployee>()
                .HasOne(te => te.Task)
                .WithMany(t => t.TaskEmployees)
                .HasForeignKey(pe => pe.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskEmployee>()
                .HasOne(te => te.Employee)
                .WithMany(e => e.TaskEmployees)
                .HasForeignKey(te => te.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectEmployee>()
                .HasKey(pe => new { pe.ProjectId, pe.EmployeeId, pe.EmployeeRoleInProject });

            modelBuilder.Entity<ProjectEmployee>()
                .HasOne(pe => pe.Project)
                .WithMany(p => p.ProjectEmployees)
                .HasForeignKey(pe => pe.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectEmployee>()
                .HasOne(pe => pe.Employee)
                .WithMany(e => e.ProjectEmployees)
                .HasForeignKey(pe => pe.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Core.Models.Task>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId);

            modelBuilder.Entity<Core.Models.Task>()
                .HasOne(t => t.TasksGroup)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.TasksGroupId);

            modelBuilder.Entity<Core.Models.Task>()
                .HasIndex(t => t.ProjectId);

            modelBuilder.Entity<Core.Models.Task>()
                .HasIndex(t => new { t.ProjectId, t.Status })
                .HasFilter("[Status] NOT IN ('Canceled', 'Completed')");

            modelBuilder.Entity<Core.Models.Task>()
                .HasIndex(t => t.TasksGroupId);

            modelBuilder.Entity<Core.Models.Task>()
                .HasIndex(t => new { t.TasksGroupId, t.Status })
                .HasFilter("[Status] NOT IN ('Canceled', 'Completed')");


            base.OnModelCreating(modelBuilder);
        }
    }
}
