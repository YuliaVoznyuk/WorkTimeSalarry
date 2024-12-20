using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkTimeSalary.Application.Interfaces;
using WorkTimeSalary.Application.Models;
using WorkTimeSalary.Domain.Entities;

namespace WorkTimeSalary.UI.Controllers
{
    public class EmployeeController(IEmployeeService employeeService) : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("ListEmployee", "Account");
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var employee = await employeeService.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return RedirectToAction("ListEmployee", "Account");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id,  EmployeeDTO employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await employeeService.UpdateAsync(employee);
                }
                catch
                {
                    return RedirectToAction(nameof(Index));
                }
               
            }
            return RedirectToAction("ListEmployee", "Account");
        }
    }
}
