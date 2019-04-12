using System;
using System.Collections.Generic;

namespace TAXIDIP1.Models
{
    public partial class Companies
    {
        public Companies()
        {
            Cars = new HashSet<Cars>();
            Drivers = new HashSet<Drivers>();
            Managers = new HashSet<Managers>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string LegalAddress { get; set; }
        public string LegalData { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdaedAt { get; set; }

        public virtual ICollection<Cars> Cars { get; set; }
        public virtual ICollection<Drivers> Drivers { get; set; }
        public virtual ICollection<Managers> Managers { get; set; }
    }
}
