using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
using AirportBusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AirportBusinessLogic.Services
{
    public class NextStationService : INextStationService<NextStation>
    {
        private readonly INextStationRepository<NextStation> _nextStationRepository;
        public NextStationService(INextStationRepository<NextStation> nextStationRepository)
        {
            _nextStationRepository= nextStationRepository;
        }

        public async Task Create(NextStation entity)
        {
            _nextStationRepository.Create(entity);
            await _nextStationRepository.SaveChangesAsync();
        }
        public NextStation? Get(int id)
        {
           return _nextStationRepository.Get(id);
        }

        public async Task<List<NextStation>> GetAll()
        {
            return await _nextStationRepository.GetAll().ToListAsync();
        }

        public bool Update(NextStation entity)
        {
            return _nextStationRepository.Update(entity);
        }
    }
}
