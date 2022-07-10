using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
using AirportBusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AirportBusinessLogic.Services
{
    public class FlightService : IFlightService<Flight>
    {
        private readonly IFlightRepository<Flight> _flightRepository;
        public FlightService(IFlightRepository<Flight> flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public void Create(Flight entity)
        {
            _flightRepository.Create(entity);
        }

        public async Task<bool> Delete(int id)
        {
           return await _flightRepository.Delete(id);
        }

        public async Task<Flight?> Get(int id)
        {
            return await _flightRepository.Get(id);
        }

        public IQueryable<Flight> GetAll()
        {
            return _flightRepository.GetAll();
        }

        public async Task SaveChangesAsync()
        {
            await _flightRepository.SaveChangesAsync();
        }

        public async Task<bool> Update(Flight entity)
        {
          return await _flightRepository.Update(entity);
        }
    }
}
