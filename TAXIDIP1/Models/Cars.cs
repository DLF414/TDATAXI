using System;
using System.Collections.Generic;

namespace TAXIDIP1.Models
{
    public partial class Cars
    {
        public int Id { get; set; }
        public int? CompanyId { get; set; }
        public int? ClaimedBy { get; set; }
        public string Car { get; set; }
        public string CarColor { get; set; }
        public string CarPlate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Drivers ClaimedByNavigation { get; set; }
        public virtual Companies Company { get; set; }
    }
}
