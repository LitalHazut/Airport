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

       
        public List<NextStation> GetPointingRoutes(Station station)
        {
            return _nextStationRepository.GetAll().
                Where(route => route.TargetId == station.StationNumber &&
                route.SourceId != null).
                ToList();
        }
        public List<NextStation> GetRoutesByCurrentStationAndAsc(int? currentStationNumber, bool isAscending)
        {
            var list2 = _nextStationRepository.GetAll().
                 Where(route => route.SourceId == currentStationNumber &&
                       route.FlightType == isAscending).ToList();
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
