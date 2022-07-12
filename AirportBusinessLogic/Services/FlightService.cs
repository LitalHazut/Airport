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

        public Task<List<Flight>> GetPendingFlightsByIsAscending(bool isAscending)
        {
            throw new NotImplementedException();
        }
        public async Task<Flight?> GetFirstFlightInQueue(List<Station> sourcesStations,bool? isFirstAscendingStation)
        {
            Flight? selectedFlight = null;

            foreach (var sourceStation in sourcesStations)
            {
                var flighyId = sourceStation.FlightId;
                if (flighyId != null)
                {
                    Flight flightToCheck = await Get((int)flighyId);
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

            if (isFirstAscendingStation != null)
            {

                var pendingList = await GetPendingFlightsByIsAscending((bool)isFirstAscendingStation);
                if (pendingList.Count != 0)
                {
                    if (selectedFlight == null) selectedFlight = pendingList[0];
                    else
                    {
                        if (selectedFlight.InsertionTime >= pendingList[0].InsertionTime) selectedFlight = pendingList[0];
                    }
                }
            }
           return selectedFlight;
        }
    }



}

