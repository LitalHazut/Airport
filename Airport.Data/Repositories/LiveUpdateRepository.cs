using Airport.Data.Contexts;
using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;

namespace Airport.Data.Repositories
{
    public class LiveUpdateRepository : ILiveUpdateRepository<LiveUpdate>
    {
        private readonly AirportContext _context;
        public LiveUpdateRepository(AirportContext context)
        {
            _context = context;
        }

        public void Create(LiveUpdate entity)
        {
            _context.Add(entity);
        }

        public bool Delete(int id)
        {
            var liveUpdate = Get(id);
            if (liveUpdate == null) return false;
            else
            {
                _context.Remove(liveUpdate);
                return true;
            }
        }

        public LiveUpdate? Get(int id)
        {
            return _context.LiveUpdates.Find(id);
        }

        public IQueryable<LiveUpdate> GetAll()
        {
            return _context.LiveUpdates;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool Update(LiveUpdate entity)
        {
            var liveUpdate = Get(entity.LiveUpdateId);
            if (liveUpdate == null) return false;
            else
            {
                _context.Update(liveUpdate);
                return true;
            }
        }
    }
}
