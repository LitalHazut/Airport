using Airport.Data.Model;


namespace AirportBusinessLogic.Interfaces
{
    public interface INextStationService<T> : ICRUDService<NextStation>
    {
        List<Station> GetSourcesStations(Station station);
    }
}
