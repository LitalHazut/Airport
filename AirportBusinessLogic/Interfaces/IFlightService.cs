using Airport.Data.Model;

namespace AirportBusinessLogic.Interfaces
{
    public interface IFlightService<T> : ICRUDService<Flight>
    {
        Task AddNewFlight(Dtos.FlightReadDto flightDto);
        IEnumerable<Dtos.FlightReadDto> GetAll();

    }
}
