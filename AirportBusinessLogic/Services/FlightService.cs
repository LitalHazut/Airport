using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
using AirportBusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AirportBusinessLogic.Services
{
    public class FlightService : IFlightService<Flight>
    {
        private readonly IFlightRepository<Flight> _flightRepository;
        public FlightService(IFlightRepository<Flight> flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public void Create(Flight entity)
        {
            entity.IsPending = true;
            entity.IsDone = false;
            entity.InsertionTime = DateTime.Now;

            _flightRepository.Create(entity);

        }

        public Flight? Get(int id)
        {
            return _flightRepository.Get(id);
        }

        public List<Flight> GetAll()
        {
            return _flightRepository.GetAll().ToList();
        }
        public bool Update(Flight entity)
        {
            return _flightRepository.Update(entity);
        }
        public Flight? GetFirstFlightInQueue(List<Station> pointingStations, bool? isFirstAscendingStation)
        {
            Flight? selectedFlight = null;
            foreach (var pointingStation in pointingStations)
            {
                var flightId = pointingStation.FlightId;
                if (flightId != null)
                {
                    Flight flightToCheck = _flightRepository.GetAll().Include(flight => flight.Stations).First(flight => flight.FlightId == (int)flightId);
                    if (flightToCheck!.TimerFinished == true)
                    {
                        if (selectedFlight == null) selectedFlight = flightToCheck;
                        else
                        {
                            if (flightToCheck.Stations.FirstOrDefault(station => station.StationNumber == 3) == null)
                            {
                                if (selectedFlight.InsertionTime >= flightToCheck!.InsertionTime) selectedFlight = flightToCheck;
                            }
                            else
                            {
                                selectedFlight = flightToCheck;
                            }

                        }
                    }
                }
            }
            //returns if its a first station in an ascendingRoute(true), descendingRoute(false) or neither(null)

            if (isFirstAscendingStation != null)
            {
                Console.WriteLine("Trying to find a plane in the list to start the route");
                var pendingFirstFlight = _flightRepository.GetAll().FirstOrDefault(flight => flight.IsAscending == isFirstAscendingStation && flight.IsPending == true);
                if (pendingFirstFlight != null)
                {
                    Console.WriteLine("Found a flight in the list");
                    if (selectedFlight == null) selectedFlight = pendingFirstFlight;
                }
                else
                {
                    Console.WriteLine("Have Not Found a flight in the list");
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
    }
}

