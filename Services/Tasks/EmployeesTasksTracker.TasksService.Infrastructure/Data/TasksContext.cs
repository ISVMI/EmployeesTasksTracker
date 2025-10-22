using Microsoft.EntityFrameworkCore;

namespace EmployeesTasksTracker.TasksService.Infrastructure.Data
{
    public class TasksContext : DbContext
    {
        public TasksContext(DbContextOptions<TasksContext> options) : base(options) { }

        public DbSet<Core.Models.Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Core.Models.Task>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Core.Models.Task>()
                .Property(p => p.Name)
                .HasColumnName("Name");

            modelBuilder.Entity<Core.Models.Task>()
                .Property(p => p.Description)
                .HasColumnName("Description");

            modelBuilder.Entity<Core.Models.Task>()
                .Property(p => p.Project)
                .HasColumnName("Project");

            modelBuilder.Entity<Core.Models.Task>()
                .Property(p => p.TasksGroup)
                .HasColumnName("TasksGroup");

            modelBuilder.Entity<Core.Models.Task>()
                .Property(p => p.Deadline)
                .HasColumnName("Deadline");

            modelBuilder.Entity<Core.Models.Task>()
                .Property(p => p.CreatedAt)
                .HasColumnName("CreatedAt");

            modelBuilder.Entity<Core.Models.Task>()
                .Property(p => p.Status)
                .HasColumnName("Status");

            modelBuilder.Entity<Core.Models.Task>()
                .Property(p => p.Priority)
                .HasColumnName("Priority");

            modelBuilder.Entity<Core.Models.Task>()
                .Property(p => p.Performers)
                .HasColumnType("jsonb");

            modelBuilder.Entity<Core.Models.Task>()
                .Property(p => p.Observers)
                .HasColumnType("jsonb");

            base.OnModelCreating(modelBuilder);
        }
    }
}
