namespace WorkTimeSalary.Domain.Entities
{
    public class Department
    {
        public Department() 
        { 
            Employees = new List<Employee>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Employee>? Employees { get; set; }
    }
}
