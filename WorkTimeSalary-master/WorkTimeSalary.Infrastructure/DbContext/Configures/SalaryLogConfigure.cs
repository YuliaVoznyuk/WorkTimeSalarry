using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkTimeSalary.Domain.Entities;

namespace WorkTimeSalary.Infrastructure.DbContext.Configures
{
    public class SalaryLogConfigure : IEntityTypeConfiguration<SalaryLog>
    {
        public void Configure(EntityTypeBuilder<SalaryLog> builder)
        {
            builder.ToTable("SalaryLog");

            builder.HasKey(x =>  x.Id);
        }
    }
}
