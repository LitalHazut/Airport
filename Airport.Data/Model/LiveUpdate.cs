using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Airport.Data.Model
{
    [Table("LiveUpdate")]
    public partial class LiveUpdate
    {
        [Key]
        public int LiveUpdateId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdateTime { get; set; }
        public bool IsEntering { get; set; }
        public int? FlightId { get; set; }
        public int? StationId { get; set; }

        [ForeignKey("FlightId")]
        [InverseProperty("LiveUpdates")]
        public virtual Flight? Flight { get; set; }
        [ForeignKey("StationId")]
        [InverseProperty("LiveUpdates")]
        public virtual Station? Station { get; set; }
    }
}
