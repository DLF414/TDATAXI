using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
namespace TAXIDIP1.Models
{
    public partial class Feedback
    {
        public string Responce { get; set; }
        public string createdBy { get; set; }
        public string Type { get; set; }
        public int Id { get; set; }
        public int RideId { get; set; }
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Длина отзыва не должна превышать 1000 символов")]
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Rides Ride { get; set; }
    }
}
