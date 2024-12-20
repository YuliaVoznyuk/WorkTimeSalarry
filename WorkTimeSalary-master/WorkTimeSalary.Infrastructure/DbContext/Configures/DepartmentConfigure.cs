using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkTimeSalary.Domain.Entities;

namespace WorkTimeSalary.Infrastructure.DbContext.Configures
{
    public class DepartmentConfigure : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departmet");

            builder.HasKey(x => x.Id);

            builder.HasData
            (
                new Department()
                {
                    Id = 1,
                    Name = "IT",
                    Description = "The IT (Information Technology) department serves as the nerve center of an organization's technological infrastructure, responsible for managing, maintaining, and optimizing the systems and networks that enable business operations. Comprising skilled professionals with expertise in various domains of technology, the IT department plays a pivotal role in ensuring the smooth functioning of digital resources and services. Here's a detailed description of the IT department's key components and functions:"
                },
                new Department()
                {
                    Id = 2,
                    Name = "Finance",
                    Description = "The Finance department within an organization plays a pivotal role in managing the monetary resources and financial activities essential for the sustenance and growth of the business. It encompasses a wide array of functions, ranging from financial planning and analysis to accounting, budgeting, treasury management, and risk assessment. The primary objective of the Finance department is to optimize the allocation of financial resources, mitigate risks, and enhance the overall financial health of the organization."
                }
            );
        }
    }
}
