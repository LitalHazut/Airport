﻿using Airport.Data.Model;
using AirportBusinessLogic.Dtos;

namespace AirportBusinessLogic.Interfaces
{
    public interface IBusinessService
    {
        Task<List<FlightReadDto>> GetFinishedRoutesHistory();
        Task AddNewFlight(FlightCreateDto flight);
        List<FlightReadDto> GetAllFlights();
        Task StartApp();
        Task<List<FlightReadDto>> GetPendingFlightsByAsc(bool isAsc);

    }
}
