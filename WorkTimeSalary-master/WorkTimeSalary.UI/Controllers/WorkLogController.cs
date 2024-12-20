using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkTimeSalary.Application.Interfaces;
using WorkTimeSalary.Application.Models;
using WorkTimeSalary.Domain.Entities;

namespace WorkTimeSalary.UI.Controllers
{
    [Authorize]
    public class WorkLogController(UserManager<Employee> userManager, IWorkLogService workLogService) : Controller
    {
        public async Task<IActionResult> Index(CancellationToken cancellation)
        {
            var id = userManager.GetUserId(User);
            var worklogs = await workLogService.GetWorkLogsByEmployeeIdAsync(int.Parse(id), cancellation);
            return View(worklogs);
        }

        [HttpPost]
        public async Task<ActionResult> Index([FromBody]WorkLogDTO workLogDTO, CancellationToken cancellation)
        {
            var id = userManager.GetUserId(User);
            workLogDTO.EmployeeId = int.Parse(id);

            await workLogService.AddAsync(workLogDTO, cancellation);

            return Ok();
        }
    }
}
