﻿using Airport.Data.Contexts;
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
        public void Create(NextStation entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<NextStation?> Get(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<NextStation> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(NextStation entity)
        {
            throw new NotImplementedException();
        }
    }
}