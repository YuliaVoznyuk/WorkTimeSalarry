using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WorkTimeSalary.Application.Interfaces;
using WorkTimeSalary.Models;

namespace WorkTimeSalary.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDepartmentService _serviceFactory;
        public HomeController(ILogger<HomeController> logger, IDepartmentService serviceFactory)
        {
            _logger = logger;
            _serviceFactory = serviceFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
