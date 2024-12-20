using Microsoft.AspNetCore.Mvc;
using WorkTimeSalary.Application.Interfaces;

namespace WorkTimeSalary.UI.Controllers
{
    public class DepartmentController(IDepartmentService departmentService) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
