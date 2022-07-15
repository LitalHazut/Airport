using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
using AirportBusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            await _liveUpdateRepository.SaveChangesAsync();
        }
        public LiveUpdate? Get(int id)
        {
            return _liveUpdateRepository.Get(id);
        }

        public async Task<List<LiveUpdate>> GetAll()
        {
            return await _liveUpdateRepository.GetAll().ToListAsync();
        }

        public bool Update(LiveUpdate entity)
        {
            return _liveUpdateRepository.Update(entity);
        }
    }
}
