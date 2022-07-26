namespace AirportBusinessLogic.Dtos
{
    public class FlightReadDto
    {
        public int FlightId { get; set; }
        public bool IsAscending { get; set; }
        public bool IsPending { get; set; }
        public bool IsDone { get; set; }
        public DateTime InsertionTime { get; set; }
    }
}
