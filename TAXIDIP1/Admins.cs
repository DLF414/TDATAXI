using System;
using System.Collections.Generic;

namespace TAXIDIP1
{
    public partial class Admins
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Accounts Account { get; set; }
    }
}
