using Airport.Data.Model;

namespace AirportBusinessLogic.Interfaces
{
    public interface IFlightService<T> : ICRUDService<Flight>
    {
        Task<Flight?> GetFirstFlightInQueue(List<Station> sourcesStations, bool? isFirstAscendingStation);
    }
}
