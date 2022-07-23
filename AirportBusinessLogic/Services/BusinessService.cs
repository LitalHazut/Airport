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
        private readonly IMapper _mapper;
        public BusinessService(IFlightService<Flight> flightService, IStationService<Station> stationService,
            ILiveUpdateService<LiveUpdate> liveUpdateService, IMapper mapper, INextStationService<NextStation> nextStationService)
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
        private async Task MoveNextIfPossible(Flight flight)
        {
            Task task = null;
            Console.WriteLine($"Flight {flight.FlightId} is trying to move next");
            Station? nextStation = null;
            var currentStation = _stationService.GetAll().FirstOrDefault(station => station.FlightId == flight.FlightId);
            int? currentStationNumber = currentStation?.StationNumber;
            var nextRoutes = _nextStationService.GetRoutesByCurrentStationAndAsc(currentStationNumber, flight.IsAscending);
            var success = false;
            if (!(_nextStationService.IsCircleOfDoom(nextRoutes) && _stationService.CircleOfDoomIsFull()))
            {
                foreach (var route in nextRoutes)
                {
                    if (!success)
                    {
                        if (route.TargetId == null)
                        {
                            success = true;
                            flight.IsDone = true;
                            _flightService.Update(flight);
                            Console.WriteLine($"success = Flight {flight.FlightId} is done");
                        }
                        else
                        {
                            nextStation = _stationService.GetAll().First(station => station.StationNumber == (int)route.TargetId);
                            Console.WriteLine($"Checking if station {nextStation.StationNumber} is empty");

                            if (nextStation.FlightId == null)
                            {
                                Console.WriteLine($"success = {nextStation.StationNumber} is empty");

                                success = true;
                                //_stationService.ChangeOccupyBy(nextStation.StationNumber, flight.FlightId);
                                _stationService.ChangeOccupyBy(nextStation.StationNumber, flight.FlightId); Console.WriteLine($"Station {nextStation.StationNumber} is now filled by {flight.FlightId}");
                            }
                            else
                                Console.WriteLine($"{nextStation.StationNumber} is not empty");
                        }

                    }
                }
            }
            else
            {
                Console.WriteLine("circle of doom");
            }

            if (success)
            {
                Console.WriteLine($"Flight {flight.FlightId} succeed");

                if (flight.IsPending)
                {
                    flight.IsPending = false;
                    _flightService.Update(flight);
                    Console.WriteLine($"Flight {flight.FlightId} started the route");
                }
                else
                {
                    if (currentStation == null) Console.WriteLine($"Flight {flight.FlightId} should be in a station but isnt ***********************");
                    LiveUpdate leavingUpdate = new() { FlightId = flight.FlightId, IsEntering = false, StationNumber = currentStation!.StationNumber, UpdateTime = DateTime.Now };
                    _liveUpdateService.Create(leavingUpdate);
                    Console.WriteLine($"Flight {flight.FlightId} left station {currentStation!.StationNumber}");
                }
                if (!flight.IsDone)
                {
                    LiveUpdate enteringUpdate = new() { FlightId = flight.FlightId, IsEntering = true, StationNumber = nextStation!.StationNumber, UpdateTime = DateTime.Now };
                    _liveUpdateService.Create(enteringUpdate);
                    Console.WriteLine($"Flight {flight.FlightId} enters station {nextStation!.StationNumber}, station {nextStation.StationNumber} is occupied by {nextStation.FlightId}");
                    task = StartTimer(flight);
                }
                else
                {
                    flight.TimerFinished = null;
                    _flightService.Update(flight);
                    Console.WriteLine($"Flight {flight.FlightId} finished the route");
                }
                if (currentStation != null)
                {
                    _stationService.ChangeOccupyBy(currentStation.StationNumber, null);
                    //OccupyStation(currentStation.StationNumber, null);
                    Console.WriteLine($"{currentStation.StationNumber} is now available, trying to find new flight to get in");
                    await SendWaitingInLineFlightIfPossible(_stationService.Get(currentStation.StationNumber)!);
                }
            }
            else
                Console.WriteLine($"Flight {flight.FlightId} hasnt managed to move next");
            if (task != null) await task;
        }
        private async Task SendWaitingInLineFlightIfPossible(Station currentStation)
        {
            var sourcesStations = _nextStationService.GetPointingStations(currentStation);
            bool? isFirstAscendingStation = _nextStationService.IsFirstAscendingStation(currentStation);
            Flight? selectedFlight;
            selectedFlight = _flightService.GetFirstFlightInQueue(sourcesStations, isFirstAscendingStation);
            if (selectedFlight != null)
            {
                Console.WriteLine($"Sending Flight {selectedFlight.FlightId} to try moving next (must work)");
                await MoveNextIfPossible(_flightService.Get(selectedFlight.FlightId)!);
            }
        }
        private async Task StartTimer(Flight flight)
        {
            flight.TimerFinished = false;
            _flightService.Update(flight);
            Console.WriteLine($"{flight.FlightId} timer started");
            var rand = new Random();
            await Task.Delay(rand.Next(500, 1500));
            Console.WriteLine($"{flight.FlightId} timer finished");
            flight.TimerFinished = true;
            _flightService.Update(flight);
            Console.WriteLine("Before Move Next function");
            await MoveNextIfPossible(flight);
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
            if (ascFirstFlight != null) allTasks.Add(MoveNextIfPossible(ascFirstFlight));
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

