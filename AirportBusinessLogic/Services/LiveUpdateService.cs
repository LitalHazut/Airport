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

        public void Create(LiveUpdate entity)
        {
            _liveUpdateRepository.Create(entity);
        }

        public async Task<bool> Delete(int id)
        {
           return await _liveUpdateRepository.Delete(id);
        }

        public async Task<LiveUpdate?> Get(int id)
        {
            return await _liveUpdateRepository.Get(id);
        }

        public IQueryable<LiveUpdate> GetAll()
        {
           return _liveUpdateRepository.GetAll();
        }

        public async Task SaveChangesAsync()
        {
            await _liveUpdateRepository.SaveChangesAsync();
        }

        public async Task<bool> Update(LiveUpdate entity)
        {
            return await _liveUpdateRepository.Update(entity);
        }
    }
}
