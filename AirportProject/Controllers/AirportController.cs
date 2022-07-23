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
        public AirportController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        [Route("[action]", Name = "StartApp")]
        [HttpPost]
        public async Task StartApp()
        {
            await _businessService.StartApp();
        }

        [Route("[action]/{isAsc:bool}")]
        [HttpGet]
        public List<FlightReadDto>GetPendingFlightsByAsc(bool isAsc)
        {
            var list =  _businessService.GetPendingFlightsByAsc(isAsc);
            return list;

        }

        [Route("[action]", Name = "StartSimulator")]
        [HttpPost]
        public async Task StartSimulator(SimulatorNumber simNumber)
        {
            await _businessService.StartSimulator(simNumber.Number);
        }

        [Route("[action]", Name = "AddNewFlight")]
        [HttpPost]
        public async Task AddNewFlight(FlightCreateDto flight)
        {
            await _businessService.AddNewFlight(flight);
        }

        [Route("[action]", Name = "SeeAllLiveUpdates")]
        [HttpGet]
        public List<LiveUpdate> SeeAllLiveUpdates()
        {
            var list =  _businessService.SeeAllLiveUpdates();
            return list;
        }

        [Route("[action]", Name = "GetAllStationsStatus")]
        [HttpGet]
        public List<Station> GetAllStationsStatus()
        {
            var list = _businessService.GetAllStationsStatus();
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
