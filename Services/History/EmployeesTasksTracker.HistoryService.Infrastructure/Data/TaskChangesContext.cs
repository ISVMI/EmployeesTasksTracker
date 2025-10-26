using EmployeesTasksTracker.HistoryService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesTasksTracker.HistoryService.Infrastructure.Data
{
    public class TaskChangesContext : DbContext
    {
        public TaskChangesContext(DbContextOptions<TaskChangesContext> options) : base (options){ }

        public DbSet<TaskChanges> TaskChanges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TaskChanges>()
                .HasKey(tc => tc.Id);

            modelBuilder.Entity<TaskChanges>()
                .Property(tc => tc.TaskId)
                .HasColumnName("TaskId");

            modelBuilder.Entity<TaskChanges>()
                .Property(tc => tc.Changes)
                .HasColumnType("jsonb");

            modelBuilder.Entity<TaskChanges>()
                .Property(tc => tc.ChangedAt)
                .HasColumnName("ChangedAt");

            base.OnModelCreating(modelBuilder);
        }
    }
}
