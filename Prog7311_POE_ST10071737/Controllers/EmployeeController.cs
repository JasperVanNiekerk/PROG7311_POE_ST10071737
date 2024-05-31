using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog7311_POE_ST10071737.DataModels;
using Prog7311_POE_ST10071737.Models;
using Prog7311_POE_ST10071737.Services;


namespace Prog7311_POE_ST10071737.Controllers
{
    /// <summary>
    /// Controller for managing employee-related actions.
    /// </summary>
    public class EmployeeController : Controller
    {
        private readonly MyDbContext myDBContext;
        private static int CurrentEmployeeID;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeController"/> class.
        /// </summary>
        /// <param name="myDBContext">The database context.</param>
        public EmployeeController(MyDbContext myDBContext)
        {
            this.myDBContext = myDBContext;
        }

        /// <summary>
        /// Displays the index view.
        /// </summary>
        /// <returns>The index view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        //___________________________________________________________________________________________________________

        /// <summary>
        /// Displays the employee login view.
        /// </summary>
        /// <returns>The employee login view.</returns>
        [HttpGet]
        public IActionResult EmployeeLogin()
        {
            return View();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the employee login.
        /// </summary>
        /// <param name="employeeLoginVM">The employee login view model.</param>
        /// <returns>The action result.</returns>
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

        /// <summary>
        /// Displays the employee home view.
        /// </summary>
        /// <returns>The employee home view.</returns>
        [HttpGet]
        public IActionResult EmployeeHome()
        {
            var model = new EmployeeHomeVM
            {
                EmployeeName = myDBContext.Employees.FirstOrDefault(e => e.EmployeeId == CurrentEmployeeID).FirstName,
                products = myDBContext.Products.ToList(),
                Categories = myDBContext.Categories.ToList(),
                Farmers = myDBContext.Farmers.ToList()
            };
            return View(model);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Filters the products based on the provided criteria.
        /// </summary>
        /// <param name="model">The EmployeeHomeVM model containing the filter criteria.</param>
        /// <returns>The EmployeeHome view with the filtered products.</returns>
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
            if (model.SortByFarmerId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.FarmerId == model.SortByFarmerId.Value);
            }

            // Populate model properties
            model.products = productsQuery.ToList();
            model.Categories = myDBContext.Categories.ToList();

            // Retrieve and assign list of farmers
            model.Farmers = myDBContext.Farmers.ToList();

            model.EmployeeName = myDBContext.Employees.FirstOrDefault(e => e.EmployeeId == CurrentEmployeeID).FirstName;

            return View("EmployeeHome", model);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Displays the farmer requests view.
        /// </summary>
        /// <returns>The farmer requests view.</returns>
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

        /// <summary>
        /// Approves a farmer request and performs necessary actions.
        /// </summary>
        /// <param name="FRID">The ID of the farmer request to approve.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        public async Task<IActionResult> ApproveRequest(int FRID)
        {
            var farmerRequest = await myDBContext.FarmerRequests.FirstOrDefaultAsync(fr => fr.RequestId == FRID);
            if (farmerRequest != null)
            {
                var password = PasswordService.GeneratePassword();
                var email = farmerRequest.Email;
                var name = farmerRequest.FirstName;
                EmailService emailService = new EmailService();
                emailService.Sender(email, name, password);

                var newFarmer = new Farmer();
                newFarmer.FirstName = name;
                newFarmer.LastName = farmerRequest.LastName;
                newFarmer.Email = email;
                newFarmer.Password = password;
                await myDBContext.AddAsync(newFarmer);

                farmerRequest.IsApproved = true;
                myDBContext.FarmerRequests.Update(farmerRequest);
                await myDBContext.SaveChangesAsync();
            }

            return RedirectToAction("FarmerRequests");
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Displays the add employees view.
        /// </summary>
        /// <returns>The add employees view.</returns>
        [HttpGet]
        public IActionResult AddEmployees()
        {
            return View();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Adds a new employee to the database.
        /// </summary>
        /// <param name="addEmployeeVM">The view model containing the employee details.</param>
        /// <returns>The action result.</returns>
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
        //___________________________________________________________________________________________________________
    }
}//john.doe@example.com', 'SecurePassword123');
 //____________________________________EOF_________________________________________________________________________