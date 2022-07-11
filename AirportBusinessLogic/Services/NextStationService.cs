﻿using Airport.Data.Model;
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

        public List<Station> GetSourcesStations(Station station)
        {
            List<Station> sourceStations = new();
            _nextStationRepository.GetAll()
           .Include(s => s.SourceId)
           .Where(s => s.TargetId == station.StationNumber && s.SourceId != null).ToList()
           .ForEach(s => sourceStations.Add(s.Source!));
            return sourceStations;

        }

        public bool? IsFirstAscendingStation(Station currentStation)
        {
            var waitingNextStation = _nextStationRepository.GetAll().
                FirstOrDefault(n => n.TargetId == currentStation.StationNumber && n.Source == null);
            return waitingNextStation == null ? null : waitingNextStation.FlightType;
        }

        
    }
}