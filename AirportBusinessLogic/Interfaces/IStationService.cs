using Airport.Data.Model;

namespace AirportBusinessLogic.Interfaces
{
    public interface IStationService<T>: ICRUDService<Station>
    {
        Task<IEnumerable<Dtos.StationReadDto>> GetAllStations();

    }
}
