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
        public async Task<LiveUpdate?> Get(int id)
        {
            return await _liveUpdateRepository.Get(id);
        }

        public async Task<IEnumerable<LiveUpdate>> GetAll()
        {
            return await _liveUpdateRepository.GetAll().ToListAsync();
        }

        public async Task<bool> Update(LiveUpdate entity)
        {
            return await _liveUpdateRepository.Update(entity);
        }
    }
}
