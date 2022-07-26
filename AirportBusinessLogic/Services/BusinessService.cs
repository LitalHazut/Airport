using Airport.Data.Model;
using AirportBusinessLogic.Dtos;
using AirportBusinessLogic.Interfaces;
using AutoMapper;

namespace AirportBusinessLogic.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly IFlightService _flightService;
        private readonly IStationService _stationService;
        private readonly ILiveUpdateService _liveUpdateService;
        private readonly INextStationService _nextStationService;
        private readonly IMapper _mapper;
        private object _lock1 = new object();
        private object _lock2 = new object();
      
        public BusinessService(IFlightService flightService, IStationService stationService,
            ILiveUpdateService liveUpdateService, IMapper mapper, INextStationService nextStationService)
        {
            _flightService = flightService;
            _stationService = stationService;
            _liveUpdateService = liveUpdateService;
            _nextStationService = nextStationService;
            _mapper = mapper;
        }
        public async Task AddNewFlight(FlightCreateDto flightDto)
        {
            var newFlight = _mapper.Map<Flight>(flightDto);
            _flightService.Create(newFlight);
            Task task = null;
            if (newFlight.FlightId == _flightService.GetAll().First(flight => flight.IsPending == true && flight.IsAscending == newFlight.IsAscending).FlightId)
            {
                task = MoveNextIfPossible(newFlight);
            }
            if (task != null) await task;
        }
        public async Task<bool> MoveNextIfPossible(Flight flight)
        {
            Task task1 = null;
            Task task2 = null;
            Console.WriteLine($"Flight {flight.FlightId} is trying to move next");
            Station? nextStation = null;
            var allOccupied = _stationService.GetAll().Where(station => station.FlightId == flight.FlightId);
            if (allOccupied.Count() > 1)
            {
                throw new Exception("More than one station same flight");
            }
            var currentStation = _stationService.GetAll().FirstOrDefault(station => station.FlightId == flight.FlightId);

            if (currentStation == null && !flight.IsPending)
            {
                throw new Exception($"Flight {flight.FlightId} that is not pending must be in a station");
            }

            int? currentStationNumber = currentStation?.StationNumber;
            var nextRoutes = _nextStationService.GetRoutesByCurrentStationAndAsc(currentStationNumber, flight.IsAscending);
            Console.WriteLine($"Flight {flight.FlightId} getting next routes from {currentStationNumber}");
            var success = false;
            lock (_lock1)
            {
                if ((_nextStationService.IsCircleOfDoom(nextRoutes) && _stationService.CircleOfDoomIsFull()) == false)
                {

                    foreach (var route in nextRoutes)
                    {
                        if (!success)
                        {
                            if (route.TargetId == null)
                            {
                                success = true;
                                flight.IsDone = true;
                                flight.TimerFinished = null;
                                _flightService.Update(flight);
                                Console.WriteLine($"success = Flight {flight.FlightId} is done");
                                Console.WriteLine($"Flight {flight.FlightId} finished the route");
                                currentStation!.FlightId = null;
                                _stationService.Update(currentStation);
                            }
                            else
                            {
                                nextStation = _stationService.GetAll().First(station => station.StationNumber == (int)route.TargetId);
                                Console.WriteLine($"Checking if station {nextStation.StationNumber} is empty");

                                if (nextStation.FlightId == null)
                                {
                                    if (currentStation != null)
                                    {

                                        currentStation.FlightId = null;
                                        _stationService.Update(currentStation);
                                        Console.WriteLine($"{currentStation.StationNumber} is now not occupied, curr.occupied: {_stationService.Get((int)currentStationNumber)!.FlightId}");
                                    }
                                    Console.WriteLine($"success = {nextStation.StationNumber} is empty");
                                    flight.IsPending = false;
                                    flight.TimerFinished = false;
                                    _flightService.Update(flight);
                                    nextStation.FlightId = flight.FlightId;
                                    _stationService.Update(nextStation);
                                    Console.WriteLine($"Station {nextStation.StationNumber} is now filled by {_stationService.Get(nextStation.StationNumber).FlightId}");
                                    success = true;


                                }
                                else
                                    Console.WriteLine($"{nextStation.StationNumber} is not empty, its occupied by {nextStation.FlightId}");
                            }
                        }
                    }
                    if (success)
                    {
                        Console.WriteLine($"Flight {flight.FlightId} succeed");
                        _stationService.GetAll().ForEach(station =>
                        {
                            Console.WriteLine($"{station.StationNumber} - {station.FlightId}");
                        });
                        if (currentStation != null)
                        {
                            LiveUpdate leavingUpdate = new() { FlightId = flight.FlightId, IsEntering = false, StationNumber = currentStation!.StationNumber, UpdateTime = DateTime.Now };
                            _liveUpdateService.Create(leavingUpdate);
                            Console.WriteLine($"Flight {flight.FlightId} left station {currentStation!.StationNumber}");
                            task1 = SendWaitingInLineFlightIfPossible(currentStation);

                        }
                        if (!flight.IsDone)
                        {
                            LiveUpdate enteringUpdate = new() { FlightId = flight.FlightId, IsEntering = true, StationNumber = nextStation!.StationNumber, UpdateTime = DateTime.Now };
                            _liveUpdateService.Create(enteringUpdate);
                            Console.WriteLine($"Flight {flight.FlightId} enters station {nextStation!.StationNumber}, station {nextStation.StationNumber} is occupied by {nextStation.FlightId}");
                            task2 = StartTimer(flight);
                        }

                    }
                    else
                    {
                        Console.WriteLine($"{flight.FlightId} not succseed");
                        foreach (var item in nextRoutes)
                        {
                            Console.WriteLine($"{item.SourceId} to {item.TargetId}");
                        }
                    }

                }
                else
                {
                    Console.WriteLine($"circle of doom************** flight {flight.FlightId} wont succeed");
                    foreach (var route in nextRoutes)
                    {
                        Console.WriteLine($"route from {route.Source} to {route.TargetId}");
                    }
                }
            }

            if (success)
            {
                if (task1 != null) await task1;
                if (task2 != null) await task2;
                return true;
            }

            return false;
        }
        private async Task<bool> SendWaitingInLineFlightIfPossible(Station currentStation)
        {
            Flight? selectedFlight = null;
            lock (_lock2)
            {
                var pointingRoutes = _nextStationService.GetPointingRoutes(currentStation);
                var pointingStations = _stationService.GetOccupiedPointingStations(pointingRoutes);
                bool? isFirstAscendingStation = _nextStationService.IsFirstAscendingStation(currentStation);
                bool isFiveOccupied = false;
                if (_stationService.Get(5) != null )
                {
                    isFiveOccupied = _stationService.Get(5)!.FlightId!=null;
                }
                selectedFlight = _flightService.GetFirstFlightInQueue(pointingStations, isFirstAscendingStation, isFiveOccupied);
                if (selectedFlight != null) selectedFlight.TimerFinished = false;
            }

            if (selectedFlight != null)
            {
                Console.WriteLine($"Sending Flight {selectedFlight.FlightId} to try moving next (must work if not doom)");
                if (await MoveNextIfPossible(selectedFlight))
                {

                    return true;
                }
                else
                {
                    if (selectedFlight.IsPending) selectedFlight.TimerFinished = null;
                    else selectedFlight.TimerFinished = true;
                }

                Console.WriteLine($"couldnt pulled {selectedFlight.FlightId}");
            }
            return false;
        }
        private async Task StartTimer(Flight flight)
        {
            flight.TimerFinished = false;
            _flightService.Update(flight);
            Console.WriteLine($"{flight.FlightId} timer started");
            var rand = new Random();
            await Task.Delay(rand.Next(300, 700));
            Console.WriteLine($"{flight.FlightId} timer finished");

            Console.WriteLine("Before Move Next function");
            if (!await MoveNextIfPossible(flight))
            {
                flight.TimerFinished = true;
                _flightService.Update(flight);
            }
        }
        public async Task StartApp()
        {
            List<Station> allStations = _stationService.GetAll();
            List<Task> allTasks = new();
            foreach (Station station in allStations)
            {
                if (station.FlightId != null)
                {
                    var flight = _flightService.GetAll().First(flight => station.FlightId == flight.FlightId);
                    var task = StartTimer(flight);
                    allTasks.Add(task);
                }
            }
            var ascFirstFlight = _flightService.GetAll().FirstOrDefault(flight => flight.IsPending == true && flight.IsAscending);
            var descFirstFlight = _flightService.GetAll().FirstOrDefault(flight => flight.IsPending == true && !flight.IsAscending);
            if (ascFirstFlight != null)
            {
                allTasks.Add(MoveNextIfPossible(ascFirstFlight));
                //second Ascending beginning station
                ascFirstFlight = _flightService.
                    GetAll().
                    FirstOrDefault(flight => flight.IsPending == true &&
                                             flight.IsAscending &&
                                             flight.FlightId != ascFirstFlight.FlightId);
                if (ascFirstFlight != null) allTasks.Add(MoveNextIfPossible(ascFirstFlight));
            }
            if (descFirstFlight != null) allTasks.Add(MoveNextIfPossible(descFirstFlight));
            await Task.WhenAll(allTasks);
        }
        public List<FlightReadDto> GetAllFlights()
        {
            var dtoFlightsList = new List<FlightReadDto>();
            _flightService.GetAll().ForEach(flight =>
            {
                var flightDto = _mapper.Map<FlightReadDto>(flight);
                dtoFlightsList.Add(flightDto);
            });
            return dtoFlightsList;
        }
        public List<FlightReadDto> GetPendingFlightsByAsc(bool isAsc)
        {
            List<FlightReadDto> listDto = new();

            var list = _flightService.GetAll().Where(flight => flight.IsPending == true && flight.IsAscending == isAsc).ToList();
            list.ForEach(flight =>
            {
                listDto.Add(_mapper.Map<FlightReadDto>(flight));
            });
            return listDto;
        }
        public List<LiveUpdate> SeeAllLiveUpdates()
        {
            return _liveUpdateService.GetAll();
        }
        public List<StationStatus> GetStationsStatusList()
        {
            return _stationService.GetStationsStatusList();
        }
        public List<Station> GetAllStationsStatus()
        {
            return _stationService.GetAll();
        }
        public async Task StartSimulator(int numOfFlights)
        {
            List<Task> list = new();
            for (int i = 0; i < numOfFlights; i++)
            {
                FlightCreateDto newFlight = new() { IsAscending = i % 2 == 0 };
                list.Add(AddNewFlight(newFlight));
            }
            await Task.WhenAll(list);
        }
    }
}

