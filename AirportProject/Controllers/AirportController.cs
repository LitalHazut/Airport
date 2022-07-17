using Airport.Data.Model;
using AirportBusinessLogic.Dtos;
using AirportBusinessLogic.Interfaces;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Mvc;


namespace AirportProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AirportController : Controller
    {
        private readonly IBusinessService _businessService;
        private bool _isWorking;

        public AirportController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        [HttpGet("GetPendingFlightsByAsc")]
        public List<FlightReadDto> GetPendingFlightsByAsc()
        {
            return _businessService.GetAllFlights();
        }


        [Route("[action]", Name = "AddNewFlightList")]
        [HttpPost]
        public async Task AddNewFlightList(int num, bool isAsc)
        {
            for (int i = 0; i < num; i++)
            {
                FlightCreateDto newFlight = new() { IsAscending = isAsc };
                AddNewFlight(newFlight);
                //_businessService.AddNewFlight(new() { IsAscending = isAsc });
            }
        }
        [Route("[action]", Name = "AddNewFlight")]
        [HttpPost]
        public async Task AddNewFlight(FlightCreateDto flight)
        {
            await _businessService.AddNewFlight(flight);
            //var addFlight = BackgroundJob.Enqueue(() => _businessService.AddNewFlight(flight));
        }

        [Route("[action]", Name = "StartApp")]
        [HttpPost]
        public async Task StartApp()
        {
            if (!_isWorking)
            {
                _isWorking = true;
                var startApp = BackgroundJob.Enqueue(() => _businessService.StartApp());
            }

        }

    }
}
