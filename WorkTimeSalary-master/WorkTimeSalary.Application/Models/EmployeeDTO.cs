using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeSalary.Application.Models
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public WorkPositionDTO WorkPosition { get; set; }
        public DepartmentDTO Department { get; set; }
        public List<WorkLogDTO> WorkLogs { get; set; }
    }
}
