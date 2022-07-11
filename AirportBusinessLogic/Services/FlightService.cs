using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
using AirportBusinessLogic.Dtos;
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
            Flight flight = new Flight()
            {
                IsAscending = entity.IsAscending,
                IsPending = true,
                IsDone = false,
                InsertionTime = DateTime.Now,

            };
            _flightRepository.Create(flight);
            await _flightRepository.SaveChangesAsync();
          
        }

        public async Task<Flight?> Get(int id)
        {
            return await _flightRepository.Get(id);
        }

        public Task<IEnumerable<Flight>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<Flight>> GetPendingFlightsByIsAscending(bool isAscending)
        {
            throw new NotImplementedException();
        }
    }



}

