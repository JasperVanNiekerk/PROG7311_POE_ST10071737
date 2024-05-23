using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog7311_POE_ST10071737.Models;
using Prog7311_POE_ST10071737.DataModels;

namespace Prog7311_POE_ST10071737.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly MyDbContext myDBContext;

        public EmployeeController(MyDbContext myDBContext)
        {
            this.myDBContext = myDBContext;
        }
        //___________________________________________________________________________________________________________

        public IActionResult Index()
        {
            return View();
        }
        //___________________________________________________________________________________________________________

        [HttpGet]
        public IActionResult EmployeeLogin()
        {
            return View();
        }
        //___________________________________________________________________________________________________________

        [HttpPost]
        public async Task<IActionResult> EmployeeLogin(EmployeeLoginVM employeeLoginVM)
        {
            if (employeeLoginVM != null)
            {
                var existingEmployee = await myDBContext.Employees.FirstOrDefaultAsync(e => e.Email == employeeLoginVM.EmployeeEmail);
                if (existingEmployee != null)
                {
                    if (existingEmployee.Password == employeeLoginVM.EmployeePassword)
                    {
                        return RedirectToAction("EmployeeHome");
                    }
                    return RedirectToAction("EmployeeLogin");
                }
                return RedirectToAction("EmployeeLogin");
            }
            return RedirectToAction("EmployeeLogin");
        }
        //___________________________________________________________________________________________________________

        [HttpGet]
        public IActionResult EmployeeHome()
        {
            return View();
        }
    }
}
//____________________________________EOF_________________________________________________________________________