using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeSalary.Application.Models;
using WorkTimeSalary.Domain.Entities;

namespace WorkTimeSalary.Application.Interfaces
{
    public interface ISalaryLogService: IService<SalaryLogDTO>
    {
        Task<IEnumerable<SalaryLog>> GetSalaryLogsByEmployeeIdAsync(int employeeId, DateTime startDate, DateTime endDate);
        Task<decimal> CalculateTotalSalaryAsync(IEnumerable<SalaryLog> salaryLogs);
    }
}
