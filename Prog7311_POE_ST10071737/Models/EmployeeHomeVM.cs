using Prog7311_POE_ST10071737.DataModels;

namespace Prog7311_POE_ST10071737.Models
{
    public class EmployeeHomeVM
    {
        public string EmployeeName { get; set; }
        public List<Product> products { get; set; }

        // Filtering criteria
        public DateTime? FilterStartDate { get; set; }
        public DateTime? FilterEndDate { get; set; }
        public int? FilterCategoryId { get; set; }
        public decimal? FilterMinPrice { get; set; }
        public decimal? FilterMaxPrice { get; set; }

        // Category list for dropdown
        public List<Category> Categories { get; set; }
    }
}
