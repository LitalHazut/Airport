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

        public async Task<bool> Delete(int id)
        {
            var station = await Get(id);
            if (station == null) return false;
            else
            {
                _context.Remove(station);
                return true;
            }

        }

        public async Task<Station?> Get(int id)
        {
          return await _context.Stations.FindAsync(id);

        }

        public IQueryable<Station> GetAll()
        {
            return _context.Stations;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Update(Station entity)
        {
            var station = await Get(entity.StationId);
            if (station == null) return false;
            else
            {
                _context.Update(station);
                await _context.SaveChangesAsync();
                return true;
            }
        }
    }
}
