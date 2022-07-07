using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;

namespace Airport.Data.Repositories
{
    public class FlightRepository : IFlightRepository<Flight>
    {
        public Task Create(Flight entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Flight> Get(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Flight> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task Update(Flight entity)
        {
            throw new NotImplementedException();
        }
    }
}
