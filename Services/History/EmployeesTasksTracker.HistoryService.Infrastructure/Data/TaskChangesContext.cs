using EmployeesTasksTracker.HistoryService.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EmployeesTasksTracker.HistoryService.Infrastructure.Data
{
    public class TaskChangesContext : DbContext
    {
        public TaskChangesContext(DbContextOptions<TaskChangesContext> options) : base(options) { }

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
                .HasColumnType("jsonb")
                .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
                );

            modelBuilder.Entity<TaskChanges>()
                .Property(tc => tc.ChangedAt)
                .HasColumnName("ChangedAt");

            base.OnModelCreating(modelBuilder);
        }
    }
}
