using Airport.Data.Model;
using AirportBusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AirportProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AirportController : Controller
    {
        private readonly IBusinessService _businessService;

        public AirportController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        [HttpGet("GetAllStations")]
        public async Task<ActionResult<IEnumerable<Station>>> GetAllStations()
        {
            var allStations = await _businessService.GetAllStation();
            return View(allStations);
        }


        [HttpGet("GetAllFlights")]
        public async Task<ActionResult<IEnumerable<Flight>>> GetAllFlights()
        {
            var allFlights = await _businessService.GetAllFlights();
            return View(allFlights);
        }
    }
}
