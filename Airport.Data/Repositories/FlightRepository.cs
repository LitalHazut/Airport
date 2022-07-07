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

        public async Task<bool> Delete(int id)
        {
            var flight = await Get(id);
            if(flight == null)  return false;
            else
            {
                _context.Remove(flight);    
                return true;
            }
        }

        public async Task<Flight?> Get(int id)
        {
            return await _context.Flights.FindAsync(id);
        }

        public IQueryable<Flight> GetAll()
        {
            return _context.Flights;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Update(Flight entity)
        {
            var flight = await Get(entity.FlightId);
            if (flight == null) return false;
            else
            {
                _context.Update(flight);
                return true;
            }
        }
    }
}
