using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkTimeSalary.Application.Interfaces;
using WorkTimeSalary.Application.Models.User;
using WorkTimeSalary.Domain.Entities;
using WorkTimeSalary.UI.Models;

namespace WorkTimeSalary.UI.Controllers
{
    public class AccountController(
        UserManager<Employee> userManager, 
        SignInManager<Employee> signInManager, 
        IDepartmentService department,
        IEmployeeService employeeService,IWorkPositionService workPositionService) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Register()
        {
            ViewBag.DropdownData = await department.GetAllAsync();
            ViewBag.DropdownDataPosition=await workPositionService.GetAllAsync();
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await userManager.FindByNameAsync(model.Username);

                    var existingClaim = (await userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "Position");
                    if (existingClaim != null)
                    {
                        await userManager.RemoveClaimAsync(user, existingClaim);
                    }
                    var workPosition = await workPositionService.GetByIdAsync(user.PositionId);
                    var positionClaim = new Claim("Position", workPosition.Name.ToString());
                    await userManager.AddClaimAsync(user, positionClaim);
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ListEmployee(CancellationToken cancellation)
        {
            var users = await employeeService.GetAllAsync(cancellation);

            return View(users);
        }

        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await employeeService.GetByIdAsync(id);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationDTO dTO)
        {
            if (ModelState.IsValid)
            {
                var user = new Employee
                {
                    UserName = dTO.FirstName,
                    Email = dTO.Email,
                    DepartmentId = dTO.DepartmentId,
                    PositionId = dTO.PositionId

                };
                var result = await userManager.CreateAsync(user, dTO.Password);

                if (result.Succeeded)
                {
                    var existingClaim = (await userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "Position");

                    if (existingClaim != null)
                    {
                        await userManager.RemoveClaimAsync(user, existingClaim);
                    }

                    var positionClaim = new Claim("Position", dTO.PositionId.ToString());
                    await userManager.AddClaimAsync(user, positionClaim);
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            ViewBag.DropdownData = await department.GetAllAsync();
            return View();
        }

            [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
