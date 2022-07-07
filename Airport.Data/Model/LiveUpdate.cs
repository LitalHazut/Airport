using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Airport.Data.Model
{
    [Keyless]
    [Table("LiveUpdate")]
    public partial class LiveUpdate
    {
        [Column(TypeName = "datetime")]
        public DateTime UpdateTime { get; set; }
        public bool IsEntering { get; set; }
        public int? FlightId { get; set; }
        public int? StationId { get; set; }

        [ForeignKey("FlightId")]
        public virtual Flight? Flight { get; set; }
        [ForeignKey("StationId")]
        public virtual Station? Station { get; set; }
    }
}
