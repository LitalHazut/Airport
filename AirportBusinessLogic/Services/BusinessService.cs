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
           await MoveToNextStationIfPossible(flightToRead);

        }
        private async Task MoveToNextStationIfPossible(Flight flight)
        {
            Station? station = null;
            var currentStation = await _stationService.GetStationByFlightId(flight.FlightId);
            int? currentStationNumber = currentStation != null ? currentStation.StationNumber : null;
            var nextRoutes = await _nextStationService.GetListNextStations(currentStationNumber, flight.IsAscending);
            var success = false;
            foreach (var route in nextRoutes)
            {
                if (!success)
                {
                    if (route.Target == null)
                    {
                        success = true;
                        flight.IsDone = true;
                    }
                    else if (route.Target.FlightId == null)
                    {
                        success = true;
                        station = route.Target;
                        route.Target.FlightId = flight.FlightId;

                        await _stationService.InsertFlight(station.StationNumber, flight.FlightId);
                    }
                }
            }

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
                        StationNumber = currentStation!.StationNumber,
                        UpdateTime = DateTime.Now
                    };
                    await _liveUpdateService.Create(leavingupdate);
                    Console.WriteLine($"Flight {flight.FlightId} left station {currentStation!.StationNumber}");

                }
                if (!flight.IsDone)
                {

                    LiveUpdate entringupdate = new LiveUpdate()
                    {
                        FlightId = flight.FlightId,
                        IsEntering = true,
                        StationNumber = station!.StationNumber,
                        UpdateTime = DateTime.Now
                    };
                    await _liveUpdateService.Create(entringupdate);
                    Console.WriteLine($"Flight {flight.FlightId} entring station {station!.StationNumber}");
                    Task timerTask = Task.Run(() => StartTimer(flight));
                }
                else
                {
                    flight.TimerFinished = null;
                    Console.WriteLine($"Flight {flight.FlightId} finished the route");
                }
                if (currentStation != null)
                {
                    await _stationService.InsertFlight(currentStation.StationNumber, null);
                    await SendWaitingInLineFlightIfPossible(currentStation);
                }
                await _context.SaveChangesAsync();
           
            }
        }

        private async Task SendWaitingInLineFlightIfPossible(Station currentStation)
        {
            var sourcesStations = _nextStationService.GetSourcesStations(currentStation);
            bool? isFirstAscendingStation = _nextStationService.IsFirstAscendingStation(currentStation);
            bool isAsc = (bool)isFirstAscendingStation!;

            var selectedFlight = await _flightService.GetFirstFlightInQueue(sourcesStations, isAsc);
            if (selectedFlight != null) await MoveToNextStationIfPossible(selectedFlight);
        }
        private async Task StartTimer(Flight flight)
        {
            flight.TimerFinished = false;
            Console.WriteLine($"{flight.FlightId} start Timer");
            var random =new Random();            
            await Task.Delay(random.Next(3000,10000));
            flight.TimerFinished = true;
            Console.WriteLine($"{flight.FlightId} stopTimer");
            await MoveToNextStationIfPossible(flight);
        }

        public async Task StartApp()
        {
            List<Station> allStations = await _stationService.GetAll();
            foreach (var station in allStations)
            {
                if (station.FlightId != null)
                {
                    var flight = await _flightService.Get((int)station.FlightId);
                    Task timerTask = Task.Run(() => StartTimer(flight!));
                }
            }
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
