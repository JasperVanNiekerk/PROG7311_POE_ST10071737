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
        private static int CurrentEmployeeID;

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
                        CurrentEmployeeID = existingEmployee.EmployeeId;
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
            var model = new EmployeeHomeVM
            {
                EmployeeName = myDBContext.Employees.FirstOrDefault(e => e.EmployeeId == CurrentEmployeeID).FirstName,
                products = myDBContext.Products.ToList(),
                Categories = myDBContext.Categories.ToList()
            };
            return View(model);
        }
        //___________________________________________________________________________________________________________

        [HttpPost]
        public IActionResult FilterProducts(EmployeeHomeVM model)
        {
            var productsQuery = myDBContext.Products.AsQueryable();

            if (model.FilterStartDate.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.ProductionDate >= model.FilterStartDate.Value);
            }

            if (model.FilterEndDate.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.ProductionDate <= model.FilterEndDate.Value);
            }

            if (model.FilterCategoryId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == model.FilterCategoryId.Value);
            }

            if (model.FilterMinPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price >= model.FilterMinPrice.Value);
            }

            if (model.FilterMaxPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price <= model.FilterMaxPrice.Value);
            }

            model.products = productsQuery.ToList();
            model.Categories = myDBContext.Categories.ToList();

            model.EmployeeName = myDBContext.Employees.FirstOrDefault(e => e.EmployeeId == CurrentEmployeeID).FirstName;

            return View("EmployeeHome", model);
        }

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
                //send email (get info from farmer request)
                var password = PasswordService.GeneratePassword();
                var email = farmerRequest.Email;
                var name = farmerRequest.FirstName;
                EmailService emailService = new EmailService();
                emailService.Sender(email, name, password);

                //update farmer table
                var newFarmer = new Farmer();
                newFarmer.FirstName = name;
                newFarmer.LastName = farmerRequest.LastName;
                newFarmer.Email = email;
                newFarmer.Password = password;
                await myDBContext.AddAsync(newFarmer);

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

        [HttpPost]
        public async Task<IActionResult> AddEmployees(AddEmployeeVM addEmployeeVM)
        {
            if (addEmployeeVM != null)
            {
                var newEmployee = new Employee();
                newEmployee.FirstName = addEmployeeVM.Name;
                newEmployee.LastName = addEmployeeVM.Surname;
                newEmployee.Email = addEmployeeVM.Email;
                newEmployee.Password = addEmployeeVM.Password;
                await myDBContext.AddAsync(newEmployee);
                await myDBContext.SaveChangesAsync();
            }
            return RedirectToAction("AddEmployees");
        }
    }
}//john.doe@example.com', 'SecurePassword123');
 //____________________________________EOF_________________________________________________________________________