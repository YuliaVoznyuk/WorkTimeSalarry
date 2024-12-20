using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
    public class WorkLogService(WorkTimeSalaryDbContext workTimeSalaryDbContext, IMapper mapper) : IWorkLogService 
    {
       
        public async Task<WorkLogDTO> GetByIdAsync(int id, CancellationToken cancellation = default)
        {
            try
            {
                var workLogo = await workTimeSalaryDbContext.Departments.FirstOrDefaultAsync(x => x.Id == id, cancellation);

                if (workLogo == null)
                {
                    throw new ArgumentNullException(nameof(workLogo));
                }

                var dto = mapper.Map<WorkLogDTO>(workLogo);

                return dto;
            }
            catch
            {
                throw;
            }
        }
       
        public async Task<List<WorkLogDTO>> GetAllAsync(CancellationToken cancellation = default)
        {
            try
            {
                return mapper.Map<List<WorkLogDTO>>(await workTimeSalaryDbContext.WorkLogs.ToListAsync(cancellation));
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
        public async Task UpdateAsync(WorkLogDTO workLog, CancellationToken cancellation = default)
        {
            await workTimeSalaryDbContext.WorkLogs.Where(w => w.Id == workLog.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(w => w.Description, workLog.Description)
                .SetProperty(w => w.StartTime, workLog.StartTime)
                .SetProperty(w => w.EndTime, workLog.EndTime)
                .SetProperty(w => w.EmployeeId, workLog.EmployeeId),                cancellation);
        }
        public async Task<IEnumerable<WorkLog>> GetWorkLogsByEmployeeIdAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            return await workTimeSalaryDbContext.WorkLogs
                .Include(x => x.Employee)
                .Where(w => w.EmployeeId == employeeId && w.StartTime >= startDate && w.EndTime <= endDate)
                .ToListAsync();
        }
        public async Task<IEnumerable<WorkLog>> GetWorkLogsByEmployeeIdAsync(int employeeId)
        {
            return await workTimeSalaryDbContext.WorkLogs
                .Include(x => x.Employee)
                .Where(w => w.EmployeeId == employeeId)
                .ToListAsync();
        }
        public async Task<TimeSpan> CalculateTotalWorkTimeAsync(IEnumerable<WorkLog> workLogs)
        {
            TimeSpan totalWorkTime = TimeSpan.Zero;

            foreach (var workLog in workLogs)
            {
                totalWorkTime += workLog.EndTime - workLog.StartTime;
            }

            return totalWorkTime;
        }

        public async Task AddAsync(WorkLogDTO model, CancellationToken cancellation = default)
        {
            await workTimeSalaryDbContext.WorkLogs.AddAsync(mapper.Map<WorkLog>(model), cancellation);

            await workTimeSalaryDbContext.SaveChangesAsync(cancellation);
        }

		public Task<List<WorkLogDTO>> GetWorkLogsByEmployeeIdAsync(int employeeId, CancellationToken cancellation = default)
		{
			throw new NotImplementedException();
		}

		async Task<List<WorkLogDTO>> IWorkLogService.GetWorkLogsByEmployeeIdAsync(int employeeId, CancellationToken cancellation)
		{
			return mapper.Map<List<WorkLogDTO>>(await workTimeSalaryDbContext.WorkLogs.Where(x => x.EmployeeId == employeeId).ToListAsync(cancellation));
		}
	}
}
