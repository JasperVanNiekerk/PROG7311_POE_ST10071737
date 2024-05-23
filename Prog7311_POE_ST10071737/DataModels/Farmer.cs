using System;
using System.Collections.Generic;

namespace Prog7311_POE_ST10071737.DataModels
{
    public partial class Farmer
    {
        public Farmer()
        {
            Products = new HashSet<Product>();
        }

        public int FarmerId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
