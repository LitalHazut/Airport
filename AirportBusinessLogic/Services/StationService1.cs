using AirportBusinessLogic.Dtos;

namespace AirportBusinessLogic.Services
{
    public class StationService1
    {
        private readonly Interfaces.IStationRepository _stationRepository;

        public StationService1()
        {

        }

        public StationService1(Interfaces.IStationRepository stationRepository)
        {
            _stationRepository = stationRepository;
        }

        public IQueryable<Station> GetAll()
        {
            var allStations= _stationRepository.GetAll();
            return allStations.Select(station => new Station(station));
        }

        public void MoveFlightToNextStation(Station station1,Station station2, ReadFlightDto p)
        {
 
        }
    }

}
