using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Airport.Data.Model
{
    [Table("Station")]
    public partial class Station: IEntity
    {
        public Station()
        {
            LiveUpdates = new HashSet<LiveUpdate>();
            NextStationSources = new HashSet<NextStation>();
            NextStationTargets = new HashSet<NextStation>();
        }

        public int? FlightId { get; set; }
        [Key]
        public int StationNumber { get; set; }

        [ForeignKey("FlightId")]
        [InverseProperty("Stations")]
        public virtual Flight? Flight { get; set; }
        [InverseProperty("StationNumberNavigation")]
        public virtual ICollection<LiveUpdate> LiveUpdates { get; set; }
        [InverseProperty("Source")]
        public virtual ICollection<NextStation> NextStationSources { get; set; }
        [InverseProperty("Target")]
        public virtual ICollection<NextStation> NextStationTargets { get; set; }
    }
}
