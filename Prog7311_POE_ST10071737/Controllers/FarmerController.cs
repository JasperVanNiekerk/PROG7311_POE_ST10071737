using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog7311_POE_ST10071737.DataModels;
using Prog7311_POE_ST10071737.Models;


namespace Prog7311_POE_ST10071737.Controllers
{
    public class FarmerController : Controller
    {
        private readonly MyDbContext myDBContext;
        private static int CurrentFarmerID;

        /// <summary>
        /// Initializes a new instance of the <see cref="FarmerController"/> class.
        /// </summary>
        /// <param name="myDBContext">The database context.</param>
        public FarmerController(MyDbContext myDBContext)
        {
            this.myDBContext = myDBContext;
        }
        //___________________________________________________________________________________________________________

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
        /// Displays the FarmerLogin view.
        /// </summary>
        /// <returns>The FarmerLogin view.</returns>
        [HttpGet]
        public IActionResult FarmerLogin()
        {
            return View();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the Farmer login.
        /// </summary>
        /// <param name="model">The farmer login view model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        public async Task<IActionResult> FarmerLogin(farmerLoginVM model)
        {
            //check if the model is valid
            if (ModelState.IsValid)
            {
                //check if the farmer exists
                var farmer = await myDBContext.Farmers.FirstOrDefaultAsync(f => f.Email == model.FarmerEmail);

                //if the farmer exists
                if (farmer != null)
                {
                    if (farmer.Password == model.FarmerPassword)
                    {
                        CurrentFarmerID = farmer.FarmerId;
                        return RedirectToAction("FarmerHome");
                    }
                    return RedirectToAction("FarmerLogin");
                }
                else
                {
                    return RedirectToAction("FarmerLogin");
                }
            }
            return RedirectToAction("FarmerLogin");
        }
        //___________________________________________________________________________________________________________


        /// <summary>
        /// Displays the FarmerRegister view.
        /// </summary>
        /// <returns>The FarmerRegister view.</returns>
        [HttpGet]
        public IActionResult FarmerRegister()
        {
            var model = new FarmerRegisterVM();
            model.RequestMade = false;
            return View(model);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Registers a new farmer.
        /// </summary>
        /// <param name="model">The FarmerRegisterVM model containing the farmer's information.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        public async Task<IActionResult> RegisterFarmer(FarmerRegisterVM model)
        {
            //collect additional data
            var requestDate = DateTime.Now;
            var approval = false;

            //create a new farmerRequest
            var farmerRequest = new FarmerRequest
            {
                FirstName = model.FarmerName,
                LastName = model.FarmerSurname,
                Email = model.FarmerEmail,
                RequestDate = requestDate,
                IsApproved = approval
            };

            //add the request to the database
            await myDBContext.AddAsync(farmerRequest);
            await myDBContext.SaveChangesAsync();

            model.RequestMade = true;
            return RedirectToAction("FarmerRegister", model);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Displays the FarmerHome view.
        /// </summary>
        /// <returns>The FarmerHome view.</returns>
        [HttpGet]
        public IActionResult FarmerHome()
        {
            var model = new FarmerHomeVM();
            model.products = myDBContext.Products.ToList();
            model.farmerName = myDBContext.Farmers.FirstOrDefault(f => f.FarmerId == CurrentFarmerID).FirstName;
            return View(model);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Displays the AddCategory view.
        /// </summary>
        /// <returns>The AddCategory view.</returns>
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Adds a new category to the database.
        /// </summary>
        /// <param name="model">The AddCategoryVM model containing the category information.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        public async Task<IActionResult> AddCategoryToDB(AddCategoryVM model)
        {
            var newCategory = new Category
            {
                CategoryName = model.CategoryName
            };

            await myDBContext.AddAsync(newCategory);
            await myDBContext.SaveChangesAsync();

            return RedirectToAction("AddProduct");
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Displays the AddProduct view.
        /// </summary>
        /// <returns>The AddProduct view.</returns>
        [HttpGet]
        public IActionResult AddProduct()
        {
            var model = new AddProductVM();
            model.catagories = myDBContext.Categories.ToList();
            model.ProductionDate = DateTime.Now;
            return View(model);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Adds a new product to the database.
        /// </summary>
        /// <param name="model">The AddProductVM model containing the product information.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        public async Task<IActionResult> AddProductToDB(AddProductVM model)
        {
            var newProduct = new Product
            {
                ProductName = model.ProductName,
                Description = model.ProductDescription,
                Price = (decimal)model.ProductPrice,
                ProductionDate = model.ProductionDate,
                CategoryId = model.CatagoryID,
                FarmerId = CurrentFarmerID
            };

            await myDBContext.AddAsync(newProduct);
            await myDBContext.SaveChangesAsync();

            return RedirectToAction("FarmerHome");
        }
        //___________________________________________________________________________________________________________
    }
}
//____________________________________EOF_________________________________________________________________________