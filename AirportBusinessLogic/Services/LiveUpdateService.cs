using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
using AirportBusinessLogic.Interfaces;

namespace AirportBusinessLogic.Services
{
    public class LiveUpdateService : ILiveUpdateService<LiveUpdate>
    {
        private readonly ILiveUpdateRepository<LiveUpdate> _liveUpdateRepository;
        public LiveUpdateService(ILiveUpdateRepository<LiveUpdate> liveUpdateRepository)
        {
            _liveUpdateRepository = liveUpdateRepository;
        }

        public async Task Create(LiveUpdate entity)
        {
            _liveUpdateRepository.Create(entity);
        }
        public async Task<LiveUpdate?> Get(int id)
        {
            return await _liveUpdateRepository.Get(id);
        }

        public Task<IEnumerable<LiveUpdate>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
