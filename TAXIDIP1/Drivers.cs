using System;
using System.Collections.Generic;

namespace TAXIDIP1
{
    public partial class Drivers
    {
        public Drivers()
        {
            Cars = new HashSet<Cars>();
            Rides = new HashSet<Rides>();
        }

        public int Id { get; set; }
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int? CompanyId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual Companies Company { get; set; }
        public virtual ICollection<Cars> Cars { get; set; }
        public virtual ICollection<Rides> Rides { get; set; }
    }
}
