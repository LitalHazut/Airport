using Airport.Data.Contexts;
using Airport.Data.Model;
using AirportBusinessLogic.Dtos;
using AirportBusinessLogic.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;


namespace AirportBusinessLogic.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly IFlightService<Flight> _flightService;
        private readonly IStationService<Station> _stationService;
        private readonly ILiveUpdateService<LiveUpdate> _liveUpdateService;
        private readonly INextStationService<NextStation> _nextStationService;
        private readonly AirportContext _context;
        private readonly IMapper _mapper;
        public BusinessService(IFlightService<Flight> flightService, IStationService<Station> stationService,
            ILiveUpdateService<LiveUpdate> liveUpdateService, AirportContext context, IMapper mapper, INextStationService<NextStation> nextStationService)
        {
            _flightService = flightService;
            _stationService = stationService;
            _liveUpdateService = liveUpdateService;
            _nextStationService = nextStationService;
            _context = context;
            _mapper = mapper;
        }

        public async Task AddNewFlight(FlightCreateDto flight)
        {
            var flightToRead = _mapper.Map<Flight>(flight);
            await _flightService.Create(flightToRead);
            MoveToNextStationIfPossible(flightToRead);

        }
        private async void MoveToNextStationIfPossible(Flight flight)
        {
            var currentStation = _context.Stations.FirstOrDefault(s => s.FlightId == flight.FlightId);
            int? stationNumber = currentStation != null ? currentStation.StationNumber : null;
            var nextRoutes = _context.NextStations
                .Include(n => n.Target)
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
                        //******* occupy next station
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
                    Console.WriteLine($"Flight {flight.FlightId} left station x");

                }
                if (flight.IsDone!)
                {
                    ///******** next station
                    LiveUpdate entringupdate = new LiveUpdate()
                    {
                        FlightId = flight.FlightId,
                        IsEntering = true,
                        StationNumber = (int)stationNumber,
                        UpdateTime = DateTime.Now
                    };
                    await _liveUpdateService.Create(entringupdate);
                    Console.WriteLine($"Flight {flight.FlightId} entring station y");
                    StartTimer(flight);
                }
                else
                {
                    flight.TimerFinished = null;
                    Console.WriteLine($"Flight {flight.FlightId} finished the route");
                }
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
            flight.TimerFinished = false;
            await Task.Delay(15000);
            flight.TimerFinished=true;
            MoveToNextStationIfPossible(flight);
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

        
    }
}
