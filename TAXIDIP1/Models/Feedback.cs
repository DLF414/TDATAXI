using System;
using System.Collections.Generic;

namespace TAXIDIP1.Models
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public int RideId { get; set; }
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Rides Ride { get; set; }
    }
}
