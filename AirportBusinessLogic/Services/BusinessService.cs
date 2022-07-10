using Airport.Data.Contexts;
using Airport.Data.Model;
using AirportBusinessLogic.Dtos;
using AirportBusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AirportBusinessLogic.Services
{
    public class BusinessService :IBusinessService 
    {
        private readonly IFlightService<Flight> _flightService;
        private readonly IStationService<StationReadDto> _stationService;
        private readonly ILiveUpdateService<LiveUpdate> _liveUpdateService;
        private readonly AirportContext _context;
        public BusinessService(IFlightService<Flight> flightService, IStationService<StationReadDto> stationService, ILiveUpdateService<LiveUpdate> liveUpdateService, AirportContext _context)
        {
            _flightService = flightService;
            _stationService = stationService;
            _liveUpdateService = liveUpdateService;
            _context = _context;
        }

        public Task AddNewFlight(FlightCreateDto flight)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FlightReadDto>> GetAllFlights()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<StationReadDto>> GetAllStationsStatus()
        {
            List<StationReadDto> listDtos = new();
            IEnumerable<StationReadDto> list = await _stationService.GetAllStations();
            return list;
        }

        public Task<IEnumerable<FlightReadDto>> GetFinishedRoutesHistory()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<int>> GetNextStations()
        {
            throw new NotImplementedException();
        }
    }
}
