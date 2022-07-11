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

        public IQueryable<Station1> GetAll()
        {
            var allStations= _stationRepository.GetAll();
            return allStations.Select(station => new Station1(station));
        }

        public void MoveFlightToNextStation(Station1 station1,Station1 station2, FlightReadDto p)
        {
 
        }
    }

}
