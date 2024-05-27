using Microsoft.AspNetCore.Mvc;

namespace Prog7311_POE_ST10071737.Controllers
{
    public class FarmerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        //___________________________________________________________________________________________________________

        [HttpGet]
        public IActionResult Farmerlogin()
        {
            return View();
        }
        //___________________________________________________________________________________________________________

        [HttpGet]
        public IActionResult FarmerRegister()
        {
            return View();
        }
        //___________________________________________________________________________________________________________

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