using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
using AirportBusinessLogic.Interfaces;

namespace AirportBusinessLogic.Services
{
    public class StationService:IStationService<Station>
    {
        private readonly IStationRepository<Station> _stationRepository;
        public StationService(IStationRepository<Station> stationRepository)
        {
            _stationRepository = stationRepository;
        }

        public void Create(Station entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Station?> Get(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Station> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Station entity)
        {
            throw new NotImplementedException();
        }
    }
}
