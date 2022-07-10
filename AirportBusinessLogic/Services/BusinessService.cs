using Airport.Data.Model;
using AirportBusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AirportBusinessLogic.Services
{
    public class BusinessService :IBusinessService 
    {
        private readonly IFlightService<Flight> _flightService;
        private readonly IStationService<Station> _stationService;
        private readonly ILiveUpdateService<LiveUpdate> _liveUpdateService;
        public BusinessService(IFlightService<Flight> flightService, IStationService<Station> stationService, ILiveUpdateService<LiveUpdate> liveUpdateService)
        {
            _flightService = flightService;
            _stationService = stationService;
            _liveUpdateService = liveUpdateService;
        }

        public Flight CreateNewFlight()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Flight>> GetAllFlights()
        {
            return await _flightService.GetAll().ToListAsync();
        }

        public async Task<IEnumerable<Station>> GetAllStation()
        {
            return await _stationService.GetAll().ToListAsync();
        }
            
   

      
    }
}
