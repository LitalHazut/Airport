using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Airport.Data.Model
{
    [Table("Station")]
    public partial class Station
    {
        public Station()
        {
            LiveUpdates = new HashSet<LiveUpdate>();
            NextStationSources = new HashSet<NextStation>();
            NextStationTargets = new HashSet<NextStation>();
        }

        [Key]
        public int StationId { get; set; }
        public int? FlightId { get; set; }

        [ForeignKey("FlightId")]
        [InverseProperty("Stations")]
        public virtual Flight? Flight { get; set; }
        [InverseProperty("Station")]
        public virtual ICollection<LiveUpdate> LiveUpdates { get; set; }
        [InverseProperty("Source")]
        public virtual ICollection<NextStation> NextStationSources { get; set; }
        [InverseProperty("Target")]
        public virtual ICollection<NextStation> NextStationTargets { get; set; }
    }
}
