using Airport.Data.Model;

namespace AirportBusinessLogic.Interfaces
{
    public interface IFlightService<T> : ICRUDService<Flight>
    {
        Task<List<Flight>> GetPendingFlightsByIsAscending(bool isAscending);
        public async Task<Flight?> GetFirstFlightInQueue(List<Station> sourcesStations, bool? isFirstAscendingStation);
    }
}
