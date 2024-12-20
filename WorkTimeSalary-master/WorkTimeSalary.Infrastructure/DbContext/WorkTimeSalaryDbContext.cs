using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection;
using WorkTimeSalary.Domain.Entities;

namespace WorkTimeSalary.Infrastructure.DbContext
{
    public class WorkTimeSalaryDbContext : IdentityDbContext<Employee, IdentityRole<int>, int>
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<WorkLog> WorkLogs { get; set; }
        public DbSet<WorkPosition> WorkPositions { get; set; }
        public DbSet<SalaryLog> SalaryLogs { get; set; }
        public DbSet<Department> Departments { get; set; }
        public WorkTimeSalaryDbContext(DbContextOptions<WorkTimeSalaryDbContext> options) : base(options) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
