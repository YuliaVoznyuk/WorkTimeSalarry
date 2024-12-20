using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkTimeSalary.Domain.Entities;

namespace WorkTimeSalary.Infrastructure.DbContext.Configures
{
    internal class WorkLogConfigure : IEntityTypeConfiguration<WorkLog>
    {
        public void Configure(EntityTypeBuilder<WorkLog> builder)
        {
            builder.ToTable("WorkLog");

            builder.HasKey(x => x.Id);
        }
    }
}
