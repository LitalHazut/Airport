using Airport.Data.Contexts;
using Airport.Data.Model;
using AirportBusinessLogic.Dtos;
using AirportBusinessLogic.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Windows.Threading;

namespace AirportBusinessLogic.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly IFlightService<Flight> _flightService;
        private readonly IStationService<StationReadDto> _stationService;
        private readonly ILiveUpdateService<LiveUpdate> _liveUpdateService;
        private readonly INextStationService<NextStation> _nextStationService;
        private readonly AirportContext _context;
        private readonly IMapper _mapper;
        private readonly Dispatcher dispatcherTimer;
        public BusinessService(IFlightService<Flight> flightService, IStationService<StationReadDto> stationService,
            ILiveUpdateService<LiveUpdate> liveUpdateService, AirportContext context, IMapper mapper, INextStationService<NextStation> nextStationService)
        {
            _flightService = flightService;
            _stationService = stationService;
            _liveUpdateService = liveUpdateService;
            _nextStationService = nextStationService;
            _context = context;
            _mapper = mapper;
            dispatcherTimer = new DispatcherTimer();
        }

        public async Task AddNewFlight(FlightCreateDto flight)
        {
            var flightToRead = _mapper.Map<Flight>(flight);
            MoveToNextStationIfPossible(flightToRead);
            await _flightService.Create(flightToRead);
        }
        private async void MoveToNextStationIfPossible(Flight flight)
        {
            var currentStation = _context.Stations.FirstOrDefault(s => s.FlightId == flight.FlightId);
            int? stationNumber = currentStation != null ? currentStation.StationNumber : null;
            var nextRoutes = _context.NextStations
                .Include(n => n.TargetId)
                .Where(n => n.SourceId == stationNumber && n.FlightType == flight.IsAscending &&
                (n.Target == null || n.Target.FlightId == null)).ToList();

            var success = false;
            nextRoutes.ForEach(n =>
            {
                if (!success)
                {
                    if (n.Target == null)
                    {
                        success = true;
                        flight.IsDone = true;
                    }
                    else if (n.Target.FlightId == null)
                    {
                        success = true;
                    }
                }
            });
            if (success)
            {
                if (flight.IsPending)
                {
                    flight.IsPending = false;
                }
                else
                {
                    LiveUpdate leavingupdate = new LiveUpdate()
                    {
                        FlightId = flight.FlightId,
                        IsEntering = false,
                        StationNumber = (int)stationNumber,
                        UpdateTime = DateTime.Now
                    };
                    await _liveUpdateService.Create(leavingupdate);
                    Console.WriteLine("Flight left station x");

                }
                if (flight.IsDone!)
                {
                    LiveUpdate entringupdate = new LiveUpdate()
                    {
                        FlightId = flight.FlightId,
                        IsEntering = true,
                        StationNumber = (int)stationNumber,
                        UpdateTime = DateTime.Now
                    };
                    await _liveUpdateService.Create(entringupdate);
                    Console.WriteLine("Flight entring station y");
                }
                else Console.WriteLine("Flight finished the route");

                if (currentStation != null)
                {
                    currentStation.FlightId = null;
                    OccupyStationIfPossible(currentStation);
                }
                await _context.SaveChangesAsync();
            }
        }

        private async void OccupyStationIfPossible(Station currentStation)
        {
            var sourcesStations = _nextStationService.GetSourcesStations(currentStation);
            bool? isFirstAscendingStation = _nextStationService.IsFirstAscendingStation(currentStation);
            bool isAsc = (bool)isFirstAscendingStation;

            var selectedFlight = await _flightService.GetFirstFlightInQueue(sourcesStations, isAsc);
            if (selectedFlight != null) MoveToNextStationIfPossible(selectedFlight);
        }
        private async void StartTimer(Flight flight)
        {
        }

        public Task<IEnumerable<FlightReadDto>> GetAllFlights()
        {
            throw new NotImplementedException();
        }

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
