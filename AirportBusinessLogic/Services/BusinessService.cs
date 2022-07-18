using Airport.Data.Contexts;
using Airport.Data.Model;
using AirportBusinessLogic.Dtos;
using AirportBusinessLogic.Interfaces;
using AutoMapper;
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
        List<Flight> _flightsCollection;
        List<Station> _stationsCollection;
        List<LiveUpdate> _liveUpdatesCollection;
        List<NextStation> _nextStationsCollection;
        private object obj = new object();
        public BusinessService(IFlightService<Flight> flightService, IStationService<Station> stationService,
            ILiveUpdateService<LiveUpdate> liveUpdateService, AirportContext context, IMapper mapper, INextStationService<NextStation> nextStationService)
        {
            _flightService = flightService;
            _stationService = stationService;
            _liveUpdateService = liveUpdateService;
            _nextStationService = nextStationService;
            _context = context;
            _mapper = mapper;
            _flightsCollection = _context.Flights.ToList();
            _stationsCollection = _context.Stations.ToList();
            _liveUpdatesCollection = _context.LiveUpdates.ToList();
            _nextStationsCollection = _context.NextStations.Include(route => route.Target).Include(route => route.Source).ToList();

        }
        public async Task AddNewFlight(FlightCreateDto flight)
        {
            List<Task> list = new();
            for (int i = 0; i < 10; i++)
            {
                var newFlight = _mapper.Map<Flight>(flight);
                ContextFunctionsLock(2, newFlight);
                _flightsCollection.Add(newFlight);
                list.Add(MoveNextIfPossible(newFlight));
            }
            await Task.WhenAll(list);
        }

        public List<NextStation> GetRoutesByCurrentStationAndAsc(int? currentStationNumber, bool isAscending)
        {
            var list2 = new List<NextStation>();
            _nextStationsCollection.ToList().ForEach(route =>
            {
                if (route.SourceId == currentStationNumber && route.FlightType == isAscending && (route.Target == null || route.Target.FlightId == null))
                    list2.Add(route);
            });
            return list2;
        }

        private async Task MoveNextIfPossible(Flight flight)
        {
            Task task = null;
            Console.WriteLine($"Flight {flight.FlightId} is trying to move next");
            Station? station = null;
            var currentStation = _stationsCollection.FirstOrDefault(station => station.FlightId == flight.FlightId);
            int? currentStationNumber = currentStation != null ? currentStation.StationNumber : null;
            var nextRoutes = GetRoutesByCurrentStationAndAsc(currentStationNumber, flight.IsAscending);

            var success = false;
            foreach (var route in nextRoutes)
            {
                if (!success)
                {
                    if (route.TargetId == null)
                    {
                        success = true;
                        flight.IsDone = true;
                        ContextFunctionsLock(4, flight);
                        Console.WriteLine($"Flight {flight.FlightId} is done");
                    }
                    else
                    {
                        station = GetStation((int)route.TargetId!);
                        Console.WriteLine($"Checking if station {station.StationNumber} is empty");

                        if (station.FlightId == null)
                        {
                            Console.WriteLine($"{station.StationNumber} is empty");

                            success = true;
                            station.FlightId = flight.FlightId;
                            ContextFunctionsLock(3, station);
                            Console.WriteLine($"Station {station.StationNumber} is now filled by {flight.FlightId}");
                        }
                        else
                            Console.WriteLine($"{station.StationNumber} is not empty");
                    }
                }
            }
            if (success)
            {
                Console.WriteLine($"Flight {flight.FlightId} succeed");
                if (flight.IsPending)
                {
                    flight.IsPending = false;
                    ContextFunctionsLock(4, flight);
                    Console.WriteLine($"Flight {flight.FlightId} started the route");
                }
                else
                {
                    LiveUpdate leavingUpdate = new LiveUpdate()
                    {
                        FlightId = flight.FlightId,
                        IsEntering = false,
                        StationNumber = currentStation!.StationNumber,
                        UpdateTime = DateTime.Now
                    };
                    _liveUpdatesCollection.Add(leavingUpdate);
                    ContextFunctionsLock(1, leavingUpdate);
                    Console.WriteLine($"Flight {flight.FlightId} left station {currentStation!.StationNumber}");
                }
                if (!flight.IsDone)
                {
                    LiveUpdate enteringUpdate = new LiveUpdate()
                    {
                        FlightId = flight.FlightId,
                        IsEntering = true,
                        StationNumber = station!.StationNumber,
                        UpdateTime = DateTime.Now
                    };
                    _liveUpdatesCollection.Add(enteringUpdate);
                    ContextFunctionsLock(1, enteringUpdate);
                    Console.WriteLine($"Flight {flight.FlightId} enters station {station!.StationNumber}, station {station.StationNumber} is occupied by {station.FlightId}");
                    task = StartTimer(flight);
                }
                else
                {
                    flight.TimerFinished = null;
                    ContextFunctionsLock(4, flight);
                    Console.WriteLine($"Flight {flight.FlightId} finished the route");
                }
                if (currentStation != null)
                {
                    currentStation.FlightId = null;
                    ContextFunctionsLock(3, currentStation);
                    Console.WriteLine($"{currentStation.StationNumber} is now available, trying to find new flight to get in");
                    await SendWaitingInLineFlightIfPossible(currentStation);
                }
            }
            else
                Console.WriteLine($"Flight {flight.FlightId} hasnt managed to move next");
            if (task != null) await task;
        }

        private async Task SendWaitingInLineFlightIfPossible(Station currentStation)
        {
            var sourcesStations = GetSourcesStations(currentStation);
            bool? isFirstAscendingStation = IsFirstAscendingStation(currentStation);
            var selectedFlight = GetFirstFlightInQueue(sourcesStations, isFirstAscendingStation);
            if (selectedFlight != null)
            {
                Console.WriteLine($"Sending Flight {selectedFlight.FlightId} to try moving next (must work)");
                await MoveNextIfPossible(selectedFlight);
            }
        }
        private async Task StartTimer(Flight flight)
        {
            flight.TimerFinished = false;
            ContextFunctionsLock(4, flight);
            Console.WriteLine($"{flight.FlightId} timer started");
            var rand = new Random();
            await Task.Delay(rand.Next(3000, 10000));
            Console.WriteLine($"{flight.FlightId} timer finished");
            flight.TimerFinished = true;
            ContextFunctionsLock(4, flight);
            Console.WriteLine("Before Move Next function");
            await MoveNextIfPossible(flight);
        }

        public async Task StartApp()
        {
            List<Station> allStations = _stationsCollection;
            List<Task> allTasks = new();
            foreach (Station station in allStations)
            {
                if (station.FlightId != null)
                {
                    var flight = _flightsCollection.First(flight => station.FlightId == flight.FlightId);
                    var task = StartTimer(flight);
                    allTasks.Add(task);
                }
            }
            await Task.WhenAll(allTasks);
        }
        void ContextFunctionsLock(int num, IEntity entity)
        {
            lock (obj)
            {
                switch (num)
                {
                    case 1:
                        SaveNewLiveUpdate((LiveUpdate)entity);
                        break;
                    case 2:
                        SaveNewFlight((Flight)entity);
                        break;
                    case 3:
                        UpdateStation((Station)entity);
                        break;
                    case 4:
                        UpdateFlight((Flight)entity);
                        break;
                }
            }

        }
        private List<Station> GetSourcesStations(Station station)
        {
            List<Station> sourceStations = new();
            _nextStationsCollection.ForEach(next =>
            {
                if (next.TargetId == station.StationNumber && next.SourceId != null)
                {
                    sourceStations.Add(next.Source!);
                }
            });
            return sourceStations;
        }

        private bool? IsFirstAscendingStation(Station currentStation)
        {
            var waitingNextStation = _nextStationsCollection
                .FirstOrDefault(n => n.TargetId == currentStation.StationNumber && n.Source == null);
            return waitingNextStation == null ? null : waitingNextStation.FlightType;
        }

        private Flight? GetFirstFlightInQueue(List<Station> pointingStations, bool? isFirstAscendingStation)
        {
            Flight? selectedFlight = null;
            foreach (var pointingStation in pointingStations)
            {
                var flightId = pointingStation.FlightId;
                if (flightId != null)
                {
                    Flight flightToCheck = _flightsCollection.First(flight => flight.FlightId == (int)flightId);
                    if (flightToCheck!.TimerFinished == true)
                    {
                        if (selectedFlight == null) selectedFlight = flightToCheck;
                        else
                        {
                            if (selectedFlight.InsertionTime >= flightToCheck!.InsertionTime) selectedFlight = flightToCheck;
                        }
                    }
                }
            }
            //returns if its a first station in an ascendingRoute(true), descendingRoute(false) or neither(null)

            if (isFirstAscendingStation != null)
            {
                var pendingFirstFlight = _flightsCollection.FirstOrDefault(flight => flight.IsAscending == isFirstAscendingStation && flight.IsPending == true);
                if (pendingFirstFlight != null)
                {
                    if (selectedFlight == null) selectedFlight = pendingFirstFlight;
                    else
                    {
                        if (selectedFlight.InsertionTime >= pendingFirstFlight.InsertionTime) selectedFlight = pendingFirstFlight;
                    }
                }
            }
            if (selectedFlight == null)
            {
                Console.WriteLine("No flight is waiting");
                return null;
            }
            else
            {
                Console.WriteLine($"{selectedFlight.FlightId} is the first line in queue");
                return selectedFlight;
            }
        }

        private Station? GetStation(int number)
        {
            lock (obj)
            {
                return _context.Stations.Find(number);

            }
        }
        private void SaveNewLiveUpdate(LiveUpdate update)
        {
            _context.LiveUpdates.Add(update);
            _context.SaveChanges();
        }
        private void SaveNewFlight(Flight flight)
        {
            flight.IsPending = true;
            flight.IsDone = false;
            flight.InsertionTime = DateTime.Now;
            _context.Flights.Add(flight);
            _context.SaveChanges();
        }
        private void UpdateStation(Station station)
        {
            _context.Stations.Update(station);
            _context.SaveChanges();

        }
        private void UpdateFlight(Flight flight)
        {
            _context.Flights.Update(flight);
            _context.SaveChanges();
        }
        public List<FlightReadDto> GetAllFlights()
        {
            var dtoFlightList = new List<FlightReadDto>();
            _flightsCollection.ForEach(flight =>
            {
                var flightDto = _mapper.Map<FlightReadDto>(flight);
                dtoFlightList.Add(flightDto);
            });
            return dtoFlightList;
        }
        public async Task<List<FlightReadDto>> GetPendingFlightsByAsc(bool isAsc)
        {
            List<FlightReadDto> listDto = new List<FlightReadDto>();
            var list = await _context.Flights
                .Where(flight => flight.IsPending == true && flight.IsAscending == isAsc)
                .ToListAsync();
            list.ForEach(flight =>
            {
                listDto.Add(_mapper.Map<FlightReadDto>(flight));

            });
            return listDto;
        }
        public async Task<List<LiveUpdate>> SeeAllLiveUpdates()
        {
            return await _liveUpdateService.GetAll();
        }

        public async Task<List<Station>> GetAllStationsStatus()
        {
            List<Station> list = await _stationService.GetAll();
            List<Station> listDtos = new();
            list.ForEach(station =>
            {
                listDtos.Add(_mapper.Map<Station>(station));
            });
            var x=6;
            return listDtos;
        }
    }
}

