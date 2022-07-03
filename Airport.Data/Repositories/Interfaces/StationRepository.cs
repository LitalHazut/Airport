
using Airport.Data.Contexts;
using Airport.Data.Model;

namespace Airport.Data.Repositories.Interfaces
{
    public class StationRepository : IStationRepository
    {
        private readonly AirportContext _context;
        public StationRepository(AirportContext context)
        {
            _context = context;
        }
        public void Create(Station station)
        {
            _context.Add(station);
            _context.SaveChanges(); 
        }

        public Station Delete(int stationId)
        {
            var tmpStation = Get(stationId);
            if (tmpStation == null) return null;
            _context.Stations.Remove(tmpStation);
            _context.SaveChanges();
            return tmpStation;
        }

        public Station Get(int id)
        {
            var tmpStation = _context.Stations.FirstOrDefault(s => s.StationId == id);
            if (tmpStation == null) return null;
            return tmpStation;

        }

        public IQueryable<Station> GetAll()
        {
            return _context.Stations;
        }

        public Station Update(Station station)
        {
            throw new NotImplementedException();
        }
    }
}
