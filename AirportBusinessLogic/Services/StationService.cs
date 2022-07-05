
using AirportBusinessLogic.Models;

namespace AirportBusinessLogic.Services
{
    public class StationService
    {
        private readonly Interfaces.IStationRepository _stationRepository;

        public StationService()
        {

        }

        public StationService(Interfaces.IStationRepository stationRepository)
        {
            _stationRepository = stationRepository;
        }

        public IQueryable<Station> GetAll()
        {
            var allStations= _stationRepository.GetAll();
            return allStations.Select(station => new Station(station));
        }
        //public Station Update(Station newStation)
        //{
          
        //}

        public void MoveFlightToNextStation(Station station1,Station station2,Plane p)
        {
            station1.SetPlane(p);
            station2.HandlePlane(p);
            
        }
    }

}
