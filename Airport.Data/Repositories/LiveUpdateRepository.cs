﻿using Airport.Data.Contexts;
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
            _context.SaveChanges();
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
            var _context = new AirportContext();
            return _context.LiveUpdates;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
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
                _context.Update(entity);
                return true;
            }
        }
    }
}
