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
            _stationRepository.Create(entity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _stationRepository.Delete(id);
        }

        public async Task<Station?> Get(int id)
        {
            return await _stationRepository.Get(id);
        }

        public IQueryable<Station> GetAll()
        {
             return _stationRepository.GetAll();
        }

        public async Task SaveChangesAsync()
        {
            await _stationRepository.SaveChangesAsync();    
        }

        public async Task<bool> Update(Station entity)
        {
            return await _stationRepository.Update(entity);
        }
    }
}
