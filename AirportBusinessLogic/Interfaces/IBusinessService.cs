using Airport.Data.Model;
using AirportBusinessLogic.Dtos;

namespace AirportBusinessLogic.Interfaces
{
    public interface IBusinessService
    {
        List<Station> GetAllStationsStatus();
        List<StationStatus> GetStationsStatusList();
        Task AddNewFlight(FlightCreateDto flight);
        List<FlightReadDto> GetAllFlights();
        Task StartApp();
        List<FlightReadDto> GetPendingFlightsByAsc(bool isAsc);
        List<LiveUpdate> SeeAllLiveUpdates();
        Task StartSimulator(int numOfFlights);
        Task<bool> MoveNextIfPossible(Flight flight);
    }
}
