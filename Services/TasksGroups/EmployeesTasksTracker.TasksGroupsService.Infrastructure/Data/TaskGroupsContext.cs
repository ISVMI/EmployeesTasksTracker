using EmployeesTasksTracker.TasksGroupsService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesTasksTracker.TasksGroupsService.Infrastructure.Data
{
    public class TaskGroupsContext : DbContext
    {
        public TaskGroupsContext(DbContextOptions<TaskGroupsContext> options) : base(options) { }

        DbSet<TaskGroup> TaskGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TaskGroup>()
                .HasKey(tg => tg.Id);

            modelBuilder.Entity<TaskGroup>()
                .Property(tg => tg.Name)
                .HasColumnName("Name");

            base.OnModelCreating(modelBuilder);
        }
    }
}
