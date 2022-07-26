using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
using AirportBusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AirportBusinessLogic.Services
{
    public class StationService : IStationService
    {
        private readonly IStationRepository _stationRepository;
        public StationService(IStationRepository stationRepository)
        {
            _stationRepository = stationRepository;
        }

        public void Create(Station entity)
        {

            _stationRepository.Create(entity);
            _stationRepository.SaveChanges();
        }
        public Station? Get(int id)
        {
            return _stationRepository.Get(id);
        }
        public List<Station> GetAll()
        {
            return _stationRepository.GetAll().ToList();
        }

        public async Task<Station?> GetStationByFlightId(int id)
        {
            return await _stationRepository.GetAll().FirstOrDefaultAsync(station => station.FlightId == id);
        }

        public bool Update(Station entity)
        {
            return _stationRepository.Update(entity);
        }
        public bool CircleOfDoomIsFull()
        {
            int count = 0;
            _stationRepository.GetAll().ToList().ForEach(station =>
            {
                if (station.StationNumber >= 4 && station.StationNumber <= 8 && station.FlightId != null) count++;
            });
            if (count >= 4) return true;
            return false;
        }
        public List<StationStatus> GetStationsStatusList()
        {
            List<StationStatus> list = new();
            _stationRepository.GetAll().Include(station => station.Flight).
                ToList().
                ForEach(station =>
                {
                    bool? isAsc = station.Flight != null ? station.Flight.IsAscending : null;
                    list.Add(new StationStatus()
                    {
                        StationNumber = station.StationNumber,
                        FlightInStation = station.FlightId,
                        IsAscending = isAsc
                    });
                });
            return list;
        }

        public List<Station> GetOccupiedPointingStations(List<NextStation> pointingRoutes)
        {
            List<Station> validPointingStations = new();

            List<Station> allStations = _stationRepository.
                GetAll().
                Include(station => station.Flight)
                .ToList();
            pointingRoutes.ForEach(route =>
            {
                var station = allStations.Find(station => station.StationNumber == route.SourceId);
                var isAsc = route.FlightType;
                if (station!.FlightId != null && station.Flight!.IsAscending == isAsc)
                    validPointingStations.Add(station);
            });
            return validPointingStations;
        }
    }
}
