using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
using AirportBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportBusinessLogic.Services
{
    public class LiveUpdateService : ILiveUpdateService<LiveUpdate>
    {
        private readonly ILiveUpdateRepository<LiveUpdate> _liveUpdateRepository;
        public LiveUpdateService(ILiveUpdateRepository<LiveUpdate> liveUpdateRepository)
        {
            _liveUpdateRepository = liveUpdateRepository;
        }

        public void Create(LiveUpdate entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<LiveUpdate?> Get(int id)
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

        public Task<bool> Update(LiveUpdate entity)
        {
            throw new NotImplementedException();
        }
    }
}
