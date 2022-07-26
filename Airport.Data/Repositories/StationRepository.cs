using Airport.Data.Contexts;
using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;

namespace Airport.Data.Repositories
{
    public class StationRepository : IStationRepository
    {
        private readonly AirportContext _context;
        public StationRepository(AirportContext context)
        {
            _context = context;
        }
        private AirportContext GetContext()
        {
            AirportContext _context = new();
            return _context;
        }
        public void Create(Station entity)
        {
            _context.Add(entity);
        }
        public bool Delete(int id)
        {
            var station = Get(id);
            if (station == null) return false;
            else
            {
                _context.Remove(station);
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
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
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
