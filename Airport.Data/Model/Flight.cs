using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Airport.Data.Model
{
    [Table("Flight")]
    public partial class Flight
    {
        public Flight()
        {
            Stations = new HashSet<Station>();
        }

        [Key]
        public int FlightId { get; set; }

        [InverseProperty("Flight")]
        public virtual ICollection<Station> Stations { get; set; }
    }
}
