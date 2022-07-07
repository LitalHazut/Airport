using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
using AirportBusinessLogic.Interfaces;

namespace AirportBusinessLogic.Services
{
    public class FlightService:IFlightService<Flight>
    {
        private readonly IFlightRepository<Flight> _flightRepository;
        public FlightService(IFlightRepository<Flight> flightRepository)
        {
            _flightRepository=flightRepository; 
        }

        public IQueryable<Flight> GetAll()
        {
            return _flightRepository.GetAll();
        }
    }
}
