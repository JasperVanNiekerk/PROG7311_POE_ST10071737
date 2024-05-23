using System;
using System.Collections.Generic;

namespace Prog7311_POE_ST10071737.DataModels
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;
        public int? CategoryId { get; set; }
        public DateTime ProductionDate { get; set; }
        public int? FarmerId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Farmer? Farmer { get; set; }
    }
}
