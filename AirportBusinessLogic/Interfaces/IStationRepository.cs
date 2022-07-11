using AirportBusinessLogic.Dtos;

namespace AirportBusinessLogic.Interfaces
{
    public interface IStationRepository : Airport.Data.Repositories.Interfaces.IStationRepository<Station1>
    {
        void Update(Station1 newStation);
    }
}
