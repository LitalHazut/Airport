using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
using Airport.Test.FakeContext;


namespace Airport.Test.FakeReopositories
{
    public class FakeNextStationRepository : INextStationRepository
    {
        public FakeNextStationRepository()
        {
         
        }
        private FakeAirportContext GetContext()
        {
            FakeAirportContext _context = new();
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
            GetContext().SaveChanges();
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

