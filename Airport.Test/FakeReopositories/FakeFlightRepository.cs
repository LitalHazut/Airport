using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
using Airport.Test.FakeContext;


namespace Airport.Test.FakeReopositories
{
    public class FakeFlightRepository : IFlightRepository
    {
        public FakeFlightRepository()
        {
            
        }
        private FakeAirportContext GetContext()
        {
            FakeAirportContext _context = new();
            return _context;
        }

        public void Create(Flight entity)
        {
            var _context = GetContext();
            _context.Add(entity);
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            var flight = Get(id);
            if (flight == null) return false;
            else
            {
                var _context = GetContext();
                _context.Remove(flight);
                return true;
            }
        }

        public Flight? Get(int id)
        {
            var _context = GetContext();
            return _context.Flights.Find(id);
        }

        public IQueryable<Flight> GetAll()
        {
            var _context = GetContext();
            return _context.Flights;
        }

        public async Task SaveChangesAsync()
        {
            var _context = GetContext();
            await _context.SaveChangesAsync();
        }

        public bool Update(Flight entity)
        {
            var _context = GetContext();
            var flight = Get(entity.FlightId);
            if (flight == null) return false;
            else
            {
                _context.Update(entity);
                _context.SaveChanges();
                return true;
            }
        }

        public void SaveChanges()
        {
            var _context = GetContext();
            _context.SaveChanges();
        }
    }
}
