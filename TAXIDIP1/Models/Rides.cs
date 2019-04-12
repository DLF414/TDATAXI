using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace TAXIDIP1.Models
{
    public partial class Rides
    {
        public int Id { get; set; }
        public int? DriverId { get; set; }
        public int ClientId { get; set; }
        public int? Distance { get; set; }
        public decimal? Price { get; set; }
        public DateTime? AcceptedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? IsAccepted { get; set; }
        public NpgsqlPoint? AddressStart { get; set; }
        public NpgsqlPoint? AddressEnd { get; set; }
        public NpgsqlPoint? AddressCurrent { get; set; }
        public bool? IsCanceled { get; set; }
        public short? Rate { get; set; }
        public bool? IsComplained { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public NpgsqlPath? Path { get; set; }

        public virtual Clients Client { get; set; }
        public virtual Drivers Driver { get; set; }
        public virtual Feedback Feedback { get; set; }
    }
}
