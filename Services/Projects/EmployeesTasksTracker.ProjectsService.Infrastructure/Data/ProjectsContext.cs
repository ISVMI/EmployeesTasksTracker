using EmployeesTasksTracker.ProjectsService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesTasksTracker.ProjectsService.Infrastructure.Data
{
    public class ProjectsContext : DbContext
    {
        public ProjectsContext(DbContextOptions<ProjectsContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectsEmployees> ProjectsEmployees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Project>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Project>()
                .Property(p => p.Name)
                .HasColumnName("Name");

            modelBuilder.Entity<Project>()
                .Property(p => p.Description)
                .HasColumnName("Description");

            modelBuilder.Entity<Project>()
                .Property(p => p.Supervisor)
                .HasColumnName("Supervisor");

            modelBuilder.Entity<Project>()
                .Property(p => p.Manager)
                .HasColumnName("Manager");

            modelBuilder.Entity<ProjectsEmployees>()
                .HasKey(pe => new { pe.ProjectId, pe.EmployeeId });

            modelBuilder.Entity<ProjectsEmployees>()
                .Property(pe => pe.IsSupervisor)
                .HasColumnName("IsSupervisor");

            base.OnModelCreating(modelBuilder);
        }

    }
}
