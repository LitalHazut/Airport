using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport.Data.Repositories
{
    public class LiveUpdateRepository : ILiveUpdateRepository<LiveUpdate>
    {
        public Task Create(LiveUpdate entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<LiveUpdate> Get(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<LiveUpdate> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task Update(LiveUpdate entity)
        {
            throw new NotImplementedException();
        }
    }
}
