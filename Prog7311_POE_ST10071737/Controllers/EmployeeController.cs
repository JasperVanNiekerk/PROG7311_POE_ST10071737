using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog7311_POE_ST10071737.DataModels;
using Prog7311_POE_ST10071737.Models;
using Prog7311_POE_ST10071737.Services;


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
        //___________________________________________________________________________________________________________

        [HttpGet]
        public IActionResult FarmerRequests()
        {
            var farmerRequests = new FarmerRequestVM
            {
                FarmerRequests = myDBContext.FarmerRequests.ToList()
            };
            return View(farmerRequests);
        }
        //___________________________________________________________________________________________________________

        [HttpPost]
        public async Task<IActionResult> ApproveRequest(int FRID)
        {
            //send email (get info from farmer request)
            var farmerRequest = await myDBContext.FarmerRequests.FirstOrDefaultAsync(fr => fr.RequestId == FRID);
            if(farmerRequest != null)
            {
                var password = PasswordService.GeneratePassword();
                var email = farmerRequest.Email;
                var name = farmerRequest.FirstName;
                EmailService emailService = new EmailService();
                emailService.Sender(email, name, password);

                //update farmer request
                farmerRequest.IsApproved = true;
                myDBContext.FarmerRequests.Update(farmerRequest);
                await myDBContext.SaveChangesAsync();
            }
            
            return RedirectToAction("FarmerRequests");
        }

        [HttpGet]
        public IActionResult AddEmployees()
        {
            return View();
        }
        //___________________________________________________________________________________________________________
    }
}//john.doe@example.com', 'SecurePassword123');
 //____________________________________EOF_________________________________________________________________________