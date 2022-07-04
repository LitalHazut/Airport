using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;

namespace Airport.Data.Repositories
{
    public class FlightRepository : IFlightRepository<Flight>
    {
        public void Create(Flight entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Flight> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
