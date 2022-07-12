using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
using AirportBusinessLogic.Dtos;
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
            Flight flight = new Flight()
            {
                IsAscending = entity.IsAscending,
                IsPending = true,
                IsDone = false,
                InsertionTime = DateTime.Now,

            };
            _flightRepository.Create(flight);
            await _flightRepository.SaveChangesAsync();

        }

        public async Task<Flight?> Get(int id)
        {
            return await _flightRepository.Get(id);
        }

        public async Task<IEnumerable<Flight>> GetAll()
        {
            return await _flightRepository.GetAll().ToListAsync();
        }

        public Task<Flight?> GetPendingFlightsByIsAscending(bool isAscending)
        {
            throw new NotImplementedException();
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
                var pendingFirstFlight = await GetPendingFlightsByIsAscending((bool)isFirstAscendingStation);
                if (pendingFirstFlight.Count != 0)
                {
                    if (selectedFlight == null) selectedFlight = pendingFirstFlight[0];
                    else
                    {
                        if (selectedFlight.InsertionTime >= pendingFirstFlight[0].InsertionTime) selectedFlight = pendingFirstFlight[0];
                    }
                }
            }
            return selectedFlight;
        }
    }



}

