using Airport.Data.Contexts;
using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Airport.Data.Repositories
{
    public class StationRepository: IStationRepository<Station>
    {
        private readonly AirportContext _context;
        public StationRepository(AirportContext context)
        {
            _context = context;
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
          return _context.Stations.Find(id);

        }

        public IQueryable<Station> GetAll()
        {
            return _context.Stations;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool Update(Station entity)
        {
            var station =  Get(entity.StationNumber);
            if (station == null) return false;
            else
            {
                _context.Update(station);
               _context.SaveChangesAsync();
                return true;
            }
        }
    }
}
