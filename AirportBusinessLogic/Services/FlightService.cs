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

        public async Task Create(Flight entity)
        {
            entity.IsPending = true;
            entity.IsDone = false;
            entity.InsertionTime = DateTime.Now;

            
            _flightRepository.Create(entity);
            await _flightRepository.SaveChangesAsync();

        }

        public Flight? Get(int id)
        {
            return  _flightRepository.Get(id);
        }

        public async Task<List<Flight>> GetAll()
        {
            return await _flightRepository.GetAll().ToListAsync();
        }
        public bool Update(Flight entity)
        {
            return _flightRepository.Update(entity);
        }
    }



}

