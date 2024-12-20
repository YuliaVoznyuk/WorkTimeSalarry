using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeSalary.Application.Interfaces;
using WorkTimeSalary.Application.Models;
using WorkTimeSalary.Domain.Entities;
using WorkTimeSalary.Infrastructure.DbContext;

namespace WorkTimeSalary.Application.Services
{
    public class EmployeeService(WorkTimeSalaryDbContext workTimeSalaryDbContext, IMapper mapper) : IEmployeeService
    {
        private readonly IWorkLogService _workLogService;
        private readonly ISalaryLogService _salaryLogService;
        public async Task<decimal> CalculateEmployeeSalaryAsync(int employeeId, DateTime startDate, DateTime endDate)
        {

            var salaryLogs = await _salaryLogService.GetSalaryLogsByEmployeeIdAsync(employeeId, startDate, endDate);
            var workLogs = await _workLogService.GetWorkLogsByEmployeeIdAsync(employeeId, startDate, endDate);
            decimal totalSalary = await CalculateTotalSalaryAsync(salaryLogs);
            TimeSpan totalWorkTime = await CalculateTotalWorkTimeAsync(workLogs);
            decimal hourlyRate = totalSalary / (decimal)totalWorkTime.TotalHours;
            decimal totalHoursWorked = (decimal)totalWorkTime.TotalHours;
            decimal totalSalaryEarned = hourlyRate * totalHoursWorked;
            return totalSalaryEarned;
        }


        private async Task<TimeSpan> CalculateTotalWorkTimeAsync(IEnumerable<WorkLog> workLogs)
        {
            TimeSpan totalWorkTime = TimeSpan.Zero;

            foreach (var workLog in workLogs)
            {
                totalWorkTime += workLog.EndTime - workLog.StartTime;
            }

            return totalWorkTime;
        }
        private async Task<decimal> CalculateTotalSalaryAsync(IEnumerable<SalaryLog> salaryLogs)
        {
            decimal totalSalary = 0;

            foreach (var salaryLog in salaryLogs)
            {
                totalSalary += salaryLog.Salary;
            }

            return totalSalary;
        }

        public async Task<EmployeeDTO> GetByIdAsync(int id, CancellationToken cancellation = default)
        {
            try
            {
                var employee = await workTimeSalaryDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id, cancellation);

                if (employee == null)
                {
                    throw new ArgumentNullException(nameof(employee));
                }

                var dto = mapper.Map<EmployeeDTO>(employee);

                return dto;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<EmployeeDTO>> GetAllAsync(CancellationToken cancellation = default)
        {
            try
            {
                return mapper.Map<List<EmployeeDTO>>(await workTimeSalaryDbContext.Employees
                    .Include(x => x.Department)
                    .Include(x => x.WorkPosition)
                    .ToListAsync(cancellation));
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellation = default)
        {
            await workTimeSalaryDbContext.Employees.Where(x => x.Id == id).ExecuteDeleteAsync(cancellation);
        }

        public async Task UpdateAsync(EmployeeDTO employee, CancellationToken cancellation = default)
        {
            try
            {
               
                await workTimeSalaryDbContext.Employees.Where(e => e.Id == employee.Id)
               .ExecuteUpdateAsync(s => s
                   .SetProperty(e => e.UserName, employee.Name),
                   cancellation);
                await workTimeSalaryDbContext.SaveChangesAsync(cancellation);
            }
            catch
            {
                throw;
            }
        }

        public async Task AddAsync(EmployeeDTO employee, CancellationToken cancellation = default)
        {
            try
            {
                await workTimeSalaryDbContext.Employees.AddAsync(mapper.Map<Employee>(employee), cancellation);
            }
            catch
            {
                throw;
            }
        }
    }
}
