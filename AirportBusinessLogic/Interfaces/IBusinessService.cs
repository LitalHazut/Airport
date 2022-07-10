using AirportBusinessLogic.Dtos;

namespace AirportBusinessLogic.Interfaces
{
    public interface IBusinessService
    {
        Task<IEnumerable<int>> GetNextStations();
        Task<IEnumerable<StationReadDto>> GetAllStationsStatus();
        Task<IEnumerable<FlightReadDto>> GetAllFlights();
        Task AddNewFlight(FlightCreateDto flight);
        Task<IEnumerable<FlightReadDto>> GetFinishedRoutesHistory();
    
    }
}
