using Airport.Data.Model;

namespace AirportBusinessLogic.Interfaces
{
    public interface IStationService<T> : ICRUDService<Station>
    {
        Task<Station?> GetStationByFlightId(int id);
        void ChangeOccupyBy(int stationNumber, int? flightId);
        List<StationStatus> GetStationsStatusList();
        bool CircleOfDoomIsFull();
        public List<Station> GetOccupiedPointingStations(List<NextStation> pointingRoutes);
    }
}
