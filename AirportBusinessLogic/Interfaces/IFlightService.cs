using Airport.Data.Model;

namespace AirportBusinessLogic.Interfaces
{
    public interface IFlightService<T> : ICRUDService<Flight>
    {
        Task<List<Flight>> GetPendingFlightsByIsAscending(bool isAscending);
    }
}
