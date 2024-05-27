using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prog7311_POE_ST10071737.DataModels;
using Prog7311_POE_ST10071737.Models;
using Prog7311_POE_ST10071737.Services;


namespace Prog7311_POE_ST10071737.Controllers
{
    public class FarmerController : Controller
    {
        private readonly MyDbContext myDBContext;

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

                //if the farmer exists41
                if (farmer != null)
                {
                    if (farmer.Password == model.FarmerPassword)
                    {
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
            var  model = new FarmerRegisterVM();
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
            return View();
        }
        //___________________________________________________________________________________________________________

        [HttpGet]
        public IActionResult FarmerProfile()
        {
            return View();
        }
        //___________________________________________________________________________________________________________

        [HttpGet]
        public IActionResult Addproduct()
        {
            return View();
        }
        //___________________________________________________________________________________________________________


    }
}
//____________________________________EOF_________________________________________________________________________