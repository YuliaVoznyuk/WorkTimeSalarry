namespace WorkTimeSalary.Domain.Entities
{
    public class WorkPosition
    {
        public WorkPosition() 
        { 
            Employees = new List<Employee>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public int TimeOfWork {  get; set; }

        public List<Employee> Employees { get; set; }
    }
}
