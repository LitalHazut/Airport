using Airport.Data.Model;

namespace AirportBusinessLogic.Interfaces
{
    public interface IStationService : ICRUDService<Station>
    {
        Task<Station?> GetStationByFlightId(int id);
        List<StationStatus> GetStationsStatusList();
        bool CircleOfDoomIsFull();
        public List<Station> GetOccupiedPointingStations(List<NextStation> pointingRoutes);
    }
}
