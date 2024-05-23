using System;
using System.Collections.Generic;

namespace Prog7311_POE_ST10071737.DataModels
{
    public partial class FarmerRequest
    {
        public int RequestId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime RequestDate { get; set; }
        public bool IsApproved { get; set; }
    }
}
