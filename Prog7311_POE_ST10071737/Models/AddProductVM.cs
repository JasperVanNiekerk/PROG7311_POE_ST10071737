using Prog7311_POE_ST10071737.DataModels;

namespace Prog7311_POE_ST10071737.Models
{
    public class AddProductVM
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public DateTime ProductionDate { get; set; }
        public int CatagoryID { get; set; }

        public List <Category> catagories { get; set; }
    }
}
