using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeSalary.Application.Models;
using WorkTimeSalary.Domain.Entities;

namespace WorkTimeSalary.Application.Interfaces
{
    public interface IWorkLogService : IService<WorkLogDTO>
    {
        Task<IEnumerable<WorkLog>> GetWorkLogsByEmployeeIdAsync(int employeeId, DateTime startDate, DateTime endDate);
        Task<List<WorkLogDTO>> GetWorkLogsByEmployeeIdAsync(int employeeId, CancellationToken cancellation = default);
        Task<TimeSpan> CalculateTotalWorkTimeAsync(IEnumerable<WorkLog> workLogs);
    }
}
