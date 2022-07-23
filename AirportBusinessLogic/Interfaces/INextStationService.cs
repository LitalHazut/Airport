using Airport.Data.Model;


namespace AirportBusinessLogic.Interfaces
{
    public interface INextStationService<T> : ICRUDService<NextStation>
    {
        List<Station> GetPointingStations(Station station);
        bool? IsFirstAscendingStation(Station currentStation);
        List<NextStation> GetRoutesByCurrentStationAndAsc(int? currentStationNumber, bool isAscending);
        bool IsCircleOfDoom(List<NextStation> nextRoutes);
        public List<NextStation> GetPointingRoutes(Station station);
    }
}
