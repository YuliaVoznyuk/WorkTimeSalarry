namespace WorkTimeSalary.Domain.Entities
{
    public class WorkLog
    {
        public int Id { get; set; }

        public string Description { get; set; } = null!;

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; } = null!;
    }
}
