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

        public FarmerController(MyDbContext myDBContext)
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
        public IActionResult FarmerLogin()
        {
            return View();
        }
        //___________________________________________________________________________________________________________

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

        [HttpGet]
        public IActionResult FarmerRegister()
        {
            var model = new FarmerRegisterVM();
            model.RequestMade = false;
            return View(model);
        }
        //___________________________________________________________________________________________________________

        [HttpPost]
        public async Task<IActionResult> RegisterFarmer(FarmerRegisterVM model)
        {
            //collect aditional data
            var requestDate = DateTime.Now;
            var aproval = false;

            //create a new farmerRequest
            var farmerRequest = new FarmerRequest
            {
                FirstName = model.FarmerName,
                LastName = model.FarmerSurname,
                Email = model.FarmerEmail,
                RequestDate = requestDate,
                IsApproved = aproval
            };

            //add the request to the database
            await myDBContext.AddAsync(farmerRequest);
            await myDBContext.SaveChangesAsync();

            model.RequestMade = true;
            return RedirectToAction("FarmerRegister", model);
        }

        [HttpGet]
        public IActionResult FarmerHome()
        {
            var model = new FarmerHomeVM();
            model.products = myDBContext.Products.ToList();
            model.farmerName = myDBContext.Farmers.FirstOrDefault(f => f.FarmerId == CurrentFarmerID).FirstName;
            return View(model);
        }
        //___________________________________________________________________________________________________________

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }
        //___________________________________________________________________________________________________________

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

        [HttpGet]
        public IActionResult AddProduct()
        {
            var model = new AddProductVM();
            model.catagories = myDBContext.Categories.ToList();
            model.ProductionDate = DateTime.Now;
            return View(model);
        }
        //___________________________________________________________________________________________________________

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