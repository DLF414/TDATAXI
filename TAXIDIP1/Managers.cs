using System;
using System.Collections.Generic;

namespace TAXIDIP1
{
    public partial class Managers
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int CompanyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Accounts Account { get; set; }
        public virtual Companies Company { get; set; }
    }
}
