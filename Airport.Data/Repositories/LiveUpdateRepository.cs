using Airport.Data.Contexts;
using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<bool> Delete(int id)
        {
            var liveUpdate = await Get(id);
            if (liveUpdate == null) return false;
            else
            {
                _context.Remove(liveUpdate);
                return true;
            }
        }

        public async Task<LiveUpdate?> Get(int id)
        {
            return await _context.LiveUpdates.FindAsync(id);
        }

        public IQueryable<LiveUpdate> GetAll()
        {
            return _context.LiveUpdates;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Update(LiveUpdate entity)
        {
            var liveUpdate = await Get(entity.LiveUpdateId);
            if (liveUpdate == null) return false;
            else
            {
                _context.Update(liveUpdate);
                return true;
            }
        }
    }
}
