using Airport.Data.Model;
using AirportBusinessLogic.Dtos;
using AirportBusinessLogic.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;


namespace AirportProject.Controllers
{
    [EnableCors("myPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class AirportController : Controller
    {
        private readonly IBusinessService _businessService;
        private bool _isWorking;

        public AirportController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        [Route("[action]", Name = "StartApp")]
        [HttpPost]
        public async Task StartApp()
        {
            if (!_isWorking)
            {
                _isWorking = true;
                await _businessService.StartApp();
            }

        }

        [Route("[action]/{isAsc:bool}")]
        [HttpGet]
        public async Task<List<FlightReadDto>> GetPendingFlightsByAsc(bool isAsc)
        {
            var list = await _businessService.GetPendingFlightsByAsc(isAsc);
            return list;

        }

        //[Route("[action]", Name = "AddNewFlightList")]
        //[HttpPost]
        //public async Task AddNewFlightList(int num, bool isAsc)
        //{
        //    for (int i = 0; i < num; i++)
        //    {
        //        FlightCreateDto newFlight = new() { IsAscending = isAsc };
        //        AddNewFlight(newFlight);
        //    }
        //}

        [Route("[action]", Name = "AddNewFlight")]
        [HttpPost]
        public async Task AddNewFlight(FlightCreateDto flight)
        {
            await _businessService.AddNewFlight(flight);
        }

        [Route("[action]", Name = "SeeAllLiveUpdates")]
        [HttpGet]
        public async Task<List<LiveUpdate>> SeeAllLiveUpdates()
        {
            var list = await _businessService.SeeAllLiveUpdates();
            return list;
        }

        [Route("[action]", Name = "GetAllStationsStatus")]
        [HttpGet]
        public async Task<List<Station>> GetAllStationsStatus()
        {
            var list = await _businessService.GetAllStationsStatus();
            return list;
        }

        [Route("[action]", Name = "GetStationsStatusList")]
        [HttpGet]
        public List<StationStatus> GetStationsStatusList()
        {
            return _businessService.GetStationsStatusList();
        }
    }
}
