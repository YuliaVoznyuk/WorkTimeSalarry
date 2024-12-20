using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkTimeSalary.Application.Interfaces;
using WorkTimeSalary.Application.Services;
using WorkTimeSalary.Domain.Entities;

namespace WorkTimeSalary.UI.Controllers
{
    public class SalaryLogController(UserManager<Employee> userManager, IWorkLogService workLogService, IEmployeeService employeeService) : Controller
    {
        private decimal salaryPerHour = 40.00m;
        public async Task<IActionResult> GetSalary(int id)
        {
            decimal totalSalary = 0;
            var employee = await employeeService.GetByIdAsync(id);
            var workLogs = await workLogService.GetWorkLogsByEmployeeIdAsync(employee.Id);
            employee.Salary = 10000.00m;
            //foreach (var workLog in workLogs)
            //{
            //    TimeSpan workDuration = workLog.EndTime - workLog.StartTime;
            //    totalSalary += employee.Salary * (decimal)workDuration.TotalHours;
            //}
            //foreach (var workLog in workLogs)
            //{
            //    TimeSpan workDuration = workLog.EndTime - workLog.StartTime;
            //    decimal hourlyRate = employee.Salary / (decimal)workDuration.TotalHours;
            //    totalSalary += salaryPerHour * (decimal)workDuration.TotalHours;
            //}
            employee.Salary = totalSalary;
            return View(workLogs);
        }
    }
}
