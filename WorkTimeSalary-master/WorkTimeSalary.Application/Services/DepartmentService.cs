using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WorkTimeSalary.Application.Interfaces;
using WorkTimeSalary.Application.Models;
using WorkTimeSalary.Domain.Entities;
using WorkTimeSalary.Infrastructure.DbContext;

namespace WorkTimeSalary.Application.Services
{
    public class DepartmentService(WorkTimeSalaryDbContext workTimeSalaryDbContext, IMapper mapper) : IDepartmentService
    {
        public async Task<DepartmentDTO> GetByIdAsync(int id, CancellationToken cancellation = default)
        {
            try
            {
                var department = await workTimeSalaryDbContext.Departments.FirstOrDefaultAsync(x => x.Id == id, cancellation);

                if (department == null)
                {
                    throw new ArgumentNullException(nameof(department));
                }

                var dto = mapper.Map<DepartmentDTO>(department);

                return dto;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<DepartmentDTO>> GetAllAsync(CancellationToken cancellation = default)
        {
            try
            {
                return mapper.Map<List<DepartmentDTO>>(await workTimeSalaryDbContext.Departments.ToListAsync(cancellation));
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellation = default)
        {
            await workTimeSalaryDbContext.Departments.Where(x => x.Id == id).ExecuteDeleteAsync(cancellation);
        }

        public async Task UpdateAsync(DepartmentDTO department, CancellationToken cancellation = default)
        {
            try
            {
                await workTimeSalaryDbContext.Departments.Where(d => d.Id == department.Id)
                .ExecuteUpdateAsync(s => s.SetProperty(d => d.Employees, t => t.Employees), cancellation);
            }
            catch
            {
                throw;
            }
        }

        public async Task AddAsync(DepartmentDTO model, CancellationToken cancellation = default)
        {
            try
            {
                await workTimeSalaryDbContext.Departments.AddAsync(mapper.Map<Department>(model), cancellation);
            }
            catch
            {
                throw;
            }
        }
    }
}
