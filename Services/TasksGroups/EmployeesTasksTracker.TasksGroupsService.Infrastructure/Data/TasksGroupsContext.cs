using EmployeesTasksTracker.TasksGroupsService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesTasksTracker.TasksGroupsService.Infrastructure.Data
{
    public class TasksGroupsContext : DbContext
    {
        public TasksGroupsContext(DbContextOptions<TasksGroupsContext> options) : base(options) { }

        public DbSet<TasksGroup> TasksGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TasksGroup>()
                .HasKey(tg => tg.Id);

            modelBuilder.Entity<TasksGroup>()
                .Property(tg => tg.Name)
                .HasColumnName("Name");

            base.OnModelCreating(modelBuilder);
        }
    }
}
