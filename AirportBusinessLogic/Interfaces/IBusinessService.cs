
using Airport.Data.Model;

namespace AirportBusinessLogic.Interfaces
{
    public interface IBusinessService
    {
        Task<IEnumerable<Station>> GetAllStation();
        Task<IEnumerable<Flight>> GetAllFlights();
         Flight CreateNewFlight();
    }
}
