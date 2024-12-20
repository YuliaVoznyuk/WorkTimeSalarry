using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeSalary.Application.Models;

namespace WorkTimeSalary.Application.Interfaces
{
    public interface IEmployeeService: IService<EmployeeDTO>
    {
        Task<decimal> CalculateEmployeeSalaryAsync(int employeeId, DateTime startDate, DateTime endDate);
    }
}
