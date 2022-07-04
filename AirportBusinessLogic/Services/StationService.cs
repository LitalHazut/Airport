using Airport.Data.Contexts;
using Airport.Data.Repositories.Interfaces;
using AirportBusinessLogic.Models;

namespace AirportBusinessLogic.Services
{
    public class StationService
    {
        private readonly IStationRepository _stationRepository;

        public StationService()
        {

        }

        public StationService(IStationRepository stationRepository)
        {
            _stationRepository = stationRepository;
        }

        public IQueryable<Station> GetAll()
        {
            var allStations= _stationRepository.GetAll();
            return allStations.Select(station => new Station(station));
        }

    }

}
