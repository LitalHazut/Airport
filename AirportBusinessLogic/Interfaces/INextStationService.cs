using Airport.Data.Model;


namespace AirportBusinessLogic.Interfaces
{
    public interface INextStationService<T> : ICRUDService<NextStation>
    {
        List<Station> GetSourcesStations(Station station);
        bool? IsFirstAscendingStation(Station currentStation);
        Task<List<NextStation>> GetListNextStations(int? cuurentStation, bool isAsc);

    }
}
