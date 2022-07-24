using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
using AirportBusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AirportBusinessLogic.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepository;
        public FlightService(IFlightRepository flightRepository)
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
        public Flight? GetFirstFlightInQueue(List<Station> pointingStations, bool? isFirstAscendingStation, bool isFiveOccupied)
        {
            //All stations are already valid (occupied and by flights who are ascending/descending according to route)
            //The stations already including the Flight property in them (OccupyByNavigation)
            Flight? selectedFlight = null;
            foreach (var pointingStation in pointingStations)
            {
                var flightToCheck = _flightRepository.GetAll().
                    First(flight => flight.FlightId == pointingStation.FlightId);

                if (flightToCheck!.TimerFinished == true)
                {
                    Console.WriteLine($"Checking flight {flightToCheck.FlightId} (timer is finished)");
                    if (selectedFlight == null) selectedFlight = flightToCheck;
                    else
                    {
                        if (pointingStation.StationNumber == 8 && isFiveOccupied)
                        {
                            selectedFlight = flightToCheck;
                        }
                        else if (selectedFlight.InsertionTime >= flightToCheck!.InsertionTime) selectedFlight = flightToCheck;
                    }
                }
            }
            //returns if its a first station in an ascendingRoute(true), descendingRoute(false) or neither(null)
            if (isFirstAscendingStation != null)
            {
                Console.WriteLine("Trying to find a plane in the list to start the route");
                var pendingFirstFlight = _flightRepository.GetAll().
                    FirstOrDefault(flight => flight.IsAscending == isFirstAscendingStation &&
                                             flight.IsPending == true &&
                                             flight.TimerFinished == null);
                if (pendingFirstFlight != null && selectedFlight == null)
                {
                    Console.WriteLine("Found a flight in the list");
                    selectedFlight = pendingFirstFlight;
                    //So there wont be 6+7 that taking the same flight while one is proccessing

                }
                else
                {
                    Console.WriteLine("Have Not Found a flight in the list");
                }
            }
            if (selectedFlight == null)
            {
                Console.WriteLine("No flight is waiting in pointing stations too (So no flight at all)");
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

