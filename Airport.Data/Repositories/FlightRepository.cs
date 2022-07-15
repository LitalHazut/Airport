using Airport.Data.Contexts;
using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;

namespace Airport.Data.Repositories
{
    public class FlightRepository : IFlightRepository<Flight>
    {
        private readonly AirportContext _context;
        public FlightRepository(AirportContext context)
        {
            _context = context;
        }

        public void Create(Flight entity)
        {
            _context.Add(entity);
        }

        public bool Delete(int id)
        {
            var flight = Get(id);
            if(flight == null)  return false;
            else
            {
                _context.Remove(flight);    
                return true;
            }
        }

        public Flight? Get(int id)
        {
            return _context.Flights.Find(id);
        }

        public IQueryable<Flight> GetAll()
        {
            return _context.Flights;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool Update(Flight entity)
        {
            var flight = Get(entity.FlightId);
            if (flight == null) return false;
            else
            {
                _context.Update(flight);
                return true;
            }
        }
    }
}
