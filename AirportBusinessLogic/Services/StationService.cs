﻿using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
using AirportBusinessLogic.Dtos;
using AirportBusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AirportBusinessLogic.Services
{
    public class StationService : IStationService<Station>
    {
        private readonly IStationRepository<Station> _stationRepository;
        public StationService(IStationRepository<Station> stationRepository)
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
            if (count == 4) return true;
            return false;
        }

        public void ChangeOccupyBy(int stationNumber, int? flightId)
        {
            var station = new Station()
            {
                FlightId=flightId,
                StationNumber=stationNumber
            };  
            _stationRepository.Update(station);
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
    }
}
