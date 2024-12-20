using Microsoft.AspNetCore.Identity;

namespace WorkTimeSalary.Domain.Entities
{
    /// <summary>
    /// Standart table of user
    /// </summary>
    public class Employee : IdentityUser<int>
    {
        public Employee() 
        { 
            WorkLogs = new List<WorkLog>();
        }

        //For testing create as decimal type, but in future need to do for example byte or somethink like byte for more security
        public decimal Salary {  get; set; }
        public int PositionId { get; set; }
        public WorkPosition WorkPosition { get; set; } = null!;
        public ICollection<WorkLog>? WorkLogs { get; set; }
        public int DepartmentId { get; set; }

        public Department Department { get; set; } = null!;

        public ICollection<SalaryLog>? SalaryLogs { get; set;}
    }
}
