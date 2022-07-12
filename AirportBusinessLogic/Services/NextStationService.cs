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

        public async Task Create(NextStation entity)
        {
            _nextStationRepository.Create(entity);
            await _nextStationRepository.SaveChangesAsync();
        }
        public async Task<NextStation?> Get(int id)
        {
           return await _nextStationRepository.Get(id);
        }

        public async Task<IEnumerable<NextStation>> GetAll()
        {
            return await _nextStationRepository.GetAll().ToListAsync();
        }

        //Get all Pointers of stations and add to list
        public List<Station> GetSourcesStations(Station station)
        {
            List<Station> sourceStations = new();
            _nextStationRepository.GetAll()
           .Include(s => s.SourceId)
           .Where(s => s.TargetId == station.StationNumber && s.SourceId != null)
           .ToList()
           .ForEach(s => sourceStations.Add(s.Source!));
            return sourceStations;

        }
        // all station that waiting outside
        public bool? IsFirstAscendingStation(Station currentStation)
        {
            var waitingNextStation = _nextStationRepository.GetAll().
                FirstOrDefault(n => n.TargetId == currentStation.StationNumber && n.Source == null);
            return waitingNextStation == null ? null : waitingNextStation.FlightType;
        }
        public async Task<List<NextStation>> GetListNextStations(int? currentStationNumber, bool isAsc)
        {
            return await _nextStationRepository
                .GetAll()
                .Include(n => n.Target)
                .Where(n => n.SourceId == currentStationNumber && n.FlightType == isAsc && 
                (n.Target == null || n.Target.FlightId == null)).ToListAsync();
          
        }

        public async Task<bool> Update(NextStation entity)
        {
            return await _nextStationRepository.Update(entity);
        }
    }
}
