using Airport.Data.Model;
using AirportBusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AirportProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightController : Controller
    {
        private readonly IFlightService<Flight> _flightService;
        public FlightController(IFlightService<Flight> flightService)
        {
            _flightService = flightService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var allFlight = _flightService.GetAll();
            return View(allFlight);
        }
    }
}
