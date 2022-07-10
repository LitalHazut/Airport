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

        public async Task SaveChangesAsync()
        {
            await _flightRepository.SaveChangesAsync();
        }

        public async Task<bool> Update(Flight entity)
        {
            return await _flightRepository.Update(entity);
        }
        public async Task AddNewFlight(FlightReadDto flightDto)
        {
            Flight flight = new Flight()
            {
                IsAscending = flightDto.IsAscending,
                IsPending = true,
                IsDone = false
               
            };
            _flightRepository.Create(flight);
            await _flightRepository.SaveChangesAsync();
        }
        public IEnumerable<FlightReadDto> GetAll()
        {
            List<FlightReadDto> flights = new();

            _flightRepository.GetAll().ToList().ForEach(flight =>
            {
                flights.Add(new FlightReadDto() { FlightId = flight.FlightId, IsAscending = flight.IsAscending });
            });
            return flights;
        }


    }
}
