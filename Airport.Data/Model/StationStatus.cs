namespace Airport.Data.Model
{
    public class StationStatus
    {
        public int StationNumber { get; set; }
        public int? FlightInStation { get; set; }
        public bool? IsAscending { get; set; }
    }
}
