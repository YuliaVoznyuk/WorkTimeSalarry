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
    public class WorkPositionService(WorkTimeSalaryDbContext workTimeSalaryDbContext, IMapper mapper) : IWorkPositionService
    {
        public async Task<WorkPositionDTO> GetByIdAsync(int id, CancellationToken cancellation = default)
        {
            try
            {
                var workPosition = await workTimeSalaryDbContext.WorkPositions.FirstOrDefaultAsync(x => x.Id == id, cancellation);

                if (workPosition == null)
                {
                    throw new ArgumentNullException(nameof(workPosition));
                }

                var dto = mapper.Map<WorkPositionDTO>(workPosition);

                return dto;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<WorkPositionDTO>> GetAllAsync(CancellationToken cancellation = default)
        {
            try
            {
                return mapper.Map<List<WorkPositionDTO>>(await workTimeSalaryDbContext.WorkPositions.ToListAsync(cancellation));
            }
            catch
            {
                throw;
            }
        }
        public async Task DeleteAsync(int id, CancellationToken cancellation = default)
        {
            await workTimeSalaryDbContext.WorkPositions.Where(x => x.Id == id).ExecuteDeleteAsync(cancellation);
        }
        public async Task UpdateAsync(WorkPositionDTO workPosition, CancellationToken cancellation = default)
        {
            try
            {
                await workTimeSalaryDbContext.WorkPositions.Where(wp => wp.Id == workPosition.Id)
          .ExecuteUpdateAsync(s => s
              .SetProperty(wp => wp.Name, workPosition.Name),
       
              cancellation);
            }
            catch
            {
                throw;
            }
        }

        public async Task AddAsync(WorkPositionDTO model, CancellationToken cancellation = default)
        {
            await workTimeSalaryDbContext.WorkPositions.AddAsync(mapper.Map<WorkPosition>(model), cancellation);
        }
    }
}
