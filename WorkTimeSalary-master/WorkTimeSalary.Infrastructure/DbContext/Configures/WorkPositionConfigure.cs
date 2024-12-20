using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkTimeSalary.Domain.Entities;

namespace WorkTimeSalary.Infrastructure.DbContext.Configures
{
    public class WorkPositionConfigure : IEntityTypeConfiguration<WorkPosition>
    {
        public void Configure(EntityTypeBuilder<WorkPosition> builder)
        {
            builder.ToTable("WorkPosition");

            builder.HasKey(x => x.Id);

            builder.HasData
            (
              new WorkPosition()
              {
                Id = 1,
                Name = "Developer",
                TimeOfWork = 160
              },
              new WorkPosition()
              {
                Id = 2,
                Name = "Manager",
                TimeOfWork =  120
              }
           );
        }
    }
}
