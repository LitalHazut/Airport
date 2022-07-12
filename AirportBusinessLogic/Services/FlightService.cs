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

        public async Task Create(Flight entity)
        {
            entity.IsPending = true;
            entity.IsDone = false;
            entity.InsertionTime = DateTime.Now;

            
            _flightRepository.Create(entity);
            await _flightRepository.SaveChangesAsync();

        }

        public async Task<Flight?> Get(int id)
        {
            return await _flightRepository.Get(id);
        }

        public async Task<List<Flight>> GetAll()
        {
            return await _flightRepository.GetAll().ToListAsync();
        }

        private Flight? GetPendingFlightsByIsAscending(bool isAscending)
        {
            return _flightRepository.GetAll().FirstOrDefault(flight => flight.IsAscending == isAscending); 
        }
        public async Task<Flight?> GetFirstFlightInQueue(List<Station> sourcesStations, bool? isFirstAscendingStation)
        {
            Flight? selectedFlight = null;

            foreach (var sourceStation in sourcesStations)
            {
                var flighyId = sourceStation.FlightId;
                if (flighyId != null)
                {
                    Flight flightToCheck = await Get((int)flighyId);
                    if (flightToCheck.TimerFinished == true)
                    {
                        if (selectedFlight == null) selectedFlight = flightToCheck;
                        else
                        {
                            if (selectedFlight.InsertionTime >= flightToCheck!.InsertionTime)
                            {
                                selectedFlight = flightToCheck;
                            }
                        }
                    }

                }
            }

            if (isFirstAscendingStation != null)
            {
                var pendingFirstFlight =  GetPendingFlightsByIsAscending((bool)isFirstAscendingStation);
                if (pendingFirstFlight != null)
                {
                    if (selectedFlight == null) selectedFlight = pendingFirstFlight;
                    else
                    {
                        if (selectedFlight.InsertionTime >= pendingFirstFlight.InsertionTime) selectedFlight = pendingFirstFlight;
                    }
                }
            }
            return selectedFlight;
        }

        public async Task<bool> Update(Flight entity)
        {
            return await _flightRepository.Update(entity);
        }
    }



}

