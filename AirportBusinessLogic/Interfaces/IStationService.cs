using Airport.Data.Model;

namespace AirportBusinessLogic.Interfaces
{
    public interface IStationService<T>: ICRUDService<Station>
    {
       Task<Station?> GetStationByFlightId(int id);
    }
}
