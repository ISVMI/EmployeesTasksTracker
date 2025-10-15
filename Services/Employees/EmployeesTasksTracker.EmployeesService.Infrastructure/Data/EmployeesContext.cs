using EmployeesTasksTracker.EmployeesService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesTasksTracker.EmployeesService.Infrastructure.Data
{
    public class EmployeesContext : DbContext
    {
        public EmployeesContext(DbContextOptions<EmployeesContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Employee>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Name)
                .HasColumnName("Name");

            modelBuilder.Entity<Employee>()
                .Property(e => e.Surname)
                .HasColumnName("Surname");

            modelBuilder.Entity<Employee>()
                .Property(e => e.Patronymic)
                .HasColumnName("Patronymic");

            modelBuilder.Entity<Employee>()
                .Property(e => e.UserName)
                .HasColumnName("UserName");

            modelBuilder.Entity<Employee>()
                .Property(e => e.Role)
                .HasColumnName("Role");

            base.OnModelCreating(modelBuilder);
        }
    }
}