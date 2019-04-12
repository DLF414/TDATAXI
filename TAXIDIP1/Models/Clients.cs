using System;
using System.Collections.Generic;

namespace TAXIDIP1.Models
{
    public partial class Clients
    {
        public Clients()
        {
            Rides = new HashSet<Rides>();
        }

        public int Id { get; set; }
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual ICollection<Rides> Rides { get; set; }
    }
}
