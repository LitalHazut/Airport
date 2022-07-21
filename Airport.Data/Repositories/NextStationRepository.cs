using Airport.Data.Contexts;
using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;

namespace Airport.Data.Repositories
{
    public class NextStationRepository : INextStationRepository<NextStation>
    {
        private readonly AirportContext _context;
        public NextStationRepository(AirportContext context)
        {
            _context = context;
        }
        private AirportContext GetContext()
        {
            AirportContext _context = new();
            return _context;
        }
        public void Create(NextStation entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public NextStation? Get(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<NextStation> GetAll()
        {
            var _context = GetContext();
            return _context.NextStations;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public bool Update(NextStation entity)
        {
            throw new NotImplementedException();
        }
    }
}
