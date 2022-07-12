﻿using Airport.Data.Model;
using Airport.Data.Repositories.Interfaces;
using AirportBusinessLogic.Dtos;
using AirportBusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AirportBusinessLogic.Services
{
    public class StationService:IStationService<Station>
    {
        private readonly IStationRepository<Station> _stationRepository;
        public StationService(IStationRepository<Station> stationRepository)
        {
            _stationRepository = stationRepository;
        }

        public async Task Create(Station entity)
        {
            
            _stationRepository.Create(entity);
            await _stationRepository.SaveChangesAsync();
        }
        public async Task<Station?> Get(int id)
        {
            return await _stationRepository.Get(id);
        }

        public async Task<IEnumerable<StationReadDto>> GetAllStations()
        {
            List<Dtos.StationReadDto> listDtos = new();
            var stationsList = await _stationRepository.GetAll().ToListAsync();
            _stationRepository.GetAll().ToList().ForEach(station =>
            {
                Dtos.StationReadDto stationDto = new() {  FlightId = station.FlightId, StationNumber = station.StationNumber };
                listDtos.Add(stationDto);
            });
            return listDtos;
        }

        public async Task<IEnumerable<Station>> GetAll()
        {
            return await _stationRepository.GetAll().ToListAsync();
        }

        
    }
}
