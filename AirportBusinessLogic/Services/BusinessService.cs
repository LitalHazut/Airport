using Airport.Data.Contexts;
using Airport.Data.Model;
using AirportBusinessLogic.Dtos;
using AirportBusinessLogic.Interfaces;
using AutoMapper;
namespace AirportBusinessLogic.Services
{
    public class BusinessService :IBusinessService 
    {
        private readonly IFlightService<Flight> _flightService;
        private readonly IStationService<StationReadDto> _stationService;
        private readonly ILiveUpdateService<LiveUpdate> _liveUpdateService;
        private readonly AirportContext _context;
        private readonly IMapper _mapper;
        public BusinessService(IFlightService<Flight> flightService, IStationService<StationReadDto> stationService, ILiveUpdateService<LiveUpdate> liveUpdateService, AirportContext _context)
        {
            _flightService = flightService;
            _stationService = stationService;
            _liveUpdateService = liveUpdateService;
            _context = _context;
        }

        public async Task AddNewFlight(FlightReadDto flight)
        {
            var flightToRead= _mapper.Map<Flight>(flight);

        }

        public Task AddNewFlight(FlightCreateDto flight)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FlightReadDto>> GetAllFlights()
        {
            throw new NotImplementedException();
        }

        //public async Task<IEnumerable<FlightReadDto>> GetAllFlights()
        //{
        //    //var flightsList = _flightService.GetAll();
        //}

        public Task<IEnumerable<StationReadDto>> GetAllStationsStatus()
        {
            throw new NotImplementedException();
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
