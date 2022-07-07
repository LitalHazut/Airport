using Airport.Data.Contexts;
using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;

namespace Airport.Data.Repositories
{
    public class StationRepository: IStationRepository<Station>
    {
        private readonly AirportContext _context;
        public StationRepository(AirportContext context)
        {
            _context = context;
        }

        public async Task Create(Station station)
        {
            _context.Add(station);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int stationId)
        {
            //var tmpStation = Get(stationId);
            //if (tmpStation == null) return null;
            //_context.Stations.Remove(tmpStation);
            //await _context.SaveChangesAsync();
            //return tmpStation;
        }

        public async Task<Station> Get(int id)
        {
            //var tmpStation = _context.Stations.FirstOrDefault(s => s.StationId == id);
            //    if (tmpStation == null) return null;
            //       return tmpStation;
            throw new NotImplementedException();

        }

        public IQueryable<Station> GetAll()
        {
            return _context.Stations;
        }

        public async Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task Update(Station newStation)
        {
            //if (newStation == null) return null;
            //_context.Stations.Update(newStation);
            //await _context.SaveChangesAsync();
            //return newStation;
        }
    }
}
