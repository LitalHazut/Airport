using AirportBusinessLogic.Models;

namespace AirportBusinessLogic.Interfaces
{
    public interface IStationRepository : Airport.Data.Repositories.Interfaces.IStationRepository<Station>
    {
        void Update(Station newStation);
    }
}
