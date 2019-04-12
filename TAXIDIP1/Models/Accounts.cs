using System;
using System.Collections.Generic;

namespace TAXIDIP1.Models
{
    public partial class Accounts
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Admins Admins { get; set; }
        public virtual Clients Clients { get; set; }
        public virtual Drivers Drivers { get; set; }
        public virtual Managers Managers { get; set; }
    }
}
