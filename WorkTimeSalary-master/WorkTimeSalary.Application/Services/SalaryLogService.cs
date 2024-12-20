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
    public class SalaryLogService(WorkTimeSalaryDbContext workTimeSalaryDbContext, IMapper mapper) : ISalaryLogService
    {
        public async Task<SalaryLogDTO> GetByIdAsync(int id, CancellationToken cancellation = default)
        {
            try
            {
                var salaryLogo = await workTimeSalaryDbContext.SalaryLogs.FirstOrDefaultAsync(x => x.Id == id, cancellation);

                if (salaryLogo == null)
                {
                    throw new ArgumentNullException(nameof(salaryLogo));
                }

                var dto = mapper.Map<SalaryLogDTO>(salaryLogo);

                return dto;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<SalaryLogDTO>> GetAllAsync(CancellationToken cancellation = default)
        {
            try
            {
                return mapper.Map<List<SalaryLogDTO>>(await workTimeSalaryDbContext.SalaryLogs.ToListAsync(cancellation));
            }
            catch
            {
                throw;
            }
        }
        public async Task DeleteAsync(int id, CancellationToken cancellation = default)
        {
            await workTimeSalaryDbContext.WorkLogs.Where(x => x.Id == id).ExecuteDeleteAsync(cancellation);
        }
        public async Task UpdateAsync(SalaryLogDTO salaryLog, CancellationToken cancellation = default)
        {
            try
            {
                await workTimeSalaryDbContext.SalaryLogs.Where(s => s.Id == salaryLog.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(s => s.Salary, salaryLog.Salary)
                .SetProperty(s => s.PaymentDate, salaryLog.PaymentDate)
                .SetProperty(s => s.EmployeeId, salaryLog.EmployeeId),
                cancellation);
            }
            catch
            {
                throw;
            }
        }
        public async Task<IEnumerable<SalaryLog>> GetSalaryLogsByEmployeeIdAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            return await workTimeSalaryDbContext.SalaryLogs
              .Where(s => s.EmployeeId == employeeId && s.PaymentDate >= startDate && s.PaymentDate <= endDate)
              .ToListAsync();
        }
        public async Task<decimal> CalculateTotalSalaryAsync(IEnumerable<SalaryLog> salaryLogs)
        {
            decimal totalSalary = 0;

            foreach (var salaryLog in salaryLogs)
            {
                totalSalary += salaryLog.Salary;
            }

            return totalSalary;
        }

        public async Task AddAsync(SalaryLogDTO salaryLog, CancellationToken cancellation)
        {
            await workTimeSalaryDbContext.SalaryLogs.AddAsync(mapper.Map<SalaryLog>(salaryLog), cancellation);
        }
    }
}
