using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
using AirportBusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AirportBusinessLogic.Services
{
    public class NextStationService : INextStationService<NextStation>
    {
        private readonly INextStationRepository<NextStation> _nextStationRepository;
        public NextStationService(INextStationRepository<NextStation> nextStationRepository)
        {
            _nextStationRepository= nextStationRepository;
        }

        public void Create(NextStation entity)
        {
            _nextStationRepository.Create(entity);
             _nextStationRepository.SaveChanges();
        }
        public NextStation? Get(int id)
        {
           return _nextStationRepository.Get(id);
        }

        public List<NextStation>GetAll()
        {
            return _nextStationRepository.GetAll().ToList();
        }

        public List<Station> GetPointingStations(Station station)
        {
            List<Station> pointingStations = new();
            _nextStationRepository.GetAll().
                Include(route => route.Source).
                Where(route => route.TargetId == station.StationNumber &&
                route.Source != null).
                ToList().
                ForEach(route => pointingStations.Add(route.Source!));
            return pointingStations;
        }

        public List<NextStation> GetRoutesByCurrentStationAndAsc(int? currentStationNumber, bool isAscending)
        {
            var list2 = new List<NextStation>();
            _nextStationRepository.GetAll().ToList().ForEach(route =>
            {
                if (route.SourceId == currentStationNumber && route.FlightType == isAscending && (route.Target == null || route.Target.FlightId == null))
                    list2.Add(route);
            });
            return list2;
        }

        public bool IsCircleOfDoom(List<NextStation> nextRoutes)
        {
            if (nextRoutes.FirstOrDefault(route => (route.TargetId == 6 || route.TargetId == 7) && route.FlightType == true ||
                 (route.TargetId == 4 && route.FlightType == false)) != null) return true;
            return false;
        }

        public bool? IsFirstAscendingStation(Station currentStation)
        {
            var waitingRoute = _nextStationRepository.GetAll().
                FirstOrDefault(route => route.TargetId == currentStation.StationNumber && route.Source == null);
            return waitingRoute == null ? null : waitingRoute.FlightType;
        }
        public bool Update(NextStation entity)
        {
            return _nextStationRepository.Update(entity);
        }
    }
}
