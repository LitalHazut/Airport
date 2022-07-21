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

        public void Create(LiveUpdate entity)
        {
            _liveUpdateRepository.Create(entity);
           _liveUpdateRepository.SaveChanges();
        }
        public LiveUpdate? Get(int id)
        {
            return _liveUpdateRepository.Get(id);
        }

        public List<LiveUpdate> GetAll()
        {
            return _liveUpdateRepository.GetAll().ToList();
        }

        public bool Update(LiveUpdate entity)
        {
            return _liveUpdateRepository.Update(entity);
        }
    }
}
