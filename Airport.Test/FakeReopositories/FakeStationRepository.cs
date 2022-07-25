using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
using Airport.Test.FakeContext;

namespace Airport.Test.FakeReopositories
{
    public class FakeStationRepository: IStationRepository
    {
        public FakeStationRepository()
        {
        }

        private FakeAirportContext GetContext()
        {
            FakeAirportContext _context = new();
            return _context;
        }
        public void Create(Station entity)
        {
            GetContext().Add(entity);
        }
        public bool Delete(int id)
        {
            var station = Get(id);
            if (station == null) return false;
            else
            {
                GetContext().Remove(station);
                return true;
            }

        }
        public Station? Get(int id)
        {
            var _context = GetContext();
            return _context.Stations.Find(id);

        }
        public IQueryable<Station> GetAll()
        {
            var context = GetContext();
            return context.Stations;
        }

        public void SaveChanges()
        {
            GetContext().SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await GetContext().SaveChangesAsync();
        }

        public bool Update(Station entity)
        {
            var _context = GetContext();
            _context.Stations.Update(entity);
            _context.SaveChanges();
            return true;
        }
    }

}
