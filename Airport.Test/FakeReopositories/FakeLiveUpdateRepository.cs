using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
using Airport.Test.FakeContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport.Test.FakeReopositories
{
    public class FakeLiveUpdateRepository : ILiveUpdateRepository
    {
        public FakeLiveUpdateRepository()
        {
           
        }

        private FakeAirportContext GetContext()
        {
            FakeAirportContext _context = new();
            return _context;
        }

        public void Create(LiveUpdate entity)
        {
            var _context = GetContext();
            _context.Add(entity);
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            var liveUpdate = Get(id);
            if (liveUpdate == null) return false;
            else
            {
                GetContext().Remove(liveUpdate);
                return true;
            }
        }

        public LiveUpdate? Get(int id)
        {
            return GetContext().LiveUpdates.Find(id);
        }

        public IQueryable<LiveUpdate> GetAll()
        {
            var _context = GetContext();
            return _context.LiveUpdates;
        }

        public void SaveChanges()
        {
            GetContext().SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await GetContext().SaveChangesAsync();
        }

        public bool Update(LiveUpdate entity)
        {
            var liveUpdate = Get(entity.LiveUpdateId);
            if (liveUpdate == null) return false;
            else
            {
                GetContext().Update(entity);
                GetContext().SaveChanges();
                return true;
            }
        }

    }
}
