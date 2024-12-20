using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkTimeSalary.Domain.Entities;

namespace WorkTimeSalary.Infrastructure.DbContext.Configures
{
    public class EmployeeConfigure : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employee");

            builder.HasKey(x => x.Id);

            builder
                .HasMany(x => x.WorkLogs)
                .WithOne(w => w.Employee)
                .HasForeignKey(x => x.EmployeeId);

            builder
                .HasOne(x => x.WorkPosition)
                .WithMany(w => w.Employees)
                .HasForeignKey(t => t.PositionId);

            builder
                .HasOne(x => x.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(t => t.DepartmentId);

            builder
                .HasMany(x => x.SalaryLogs)
                .WithOne(e => e.Employee)
                .HasForeignKey(t => t.EmployeeId);
        }
    }
}
