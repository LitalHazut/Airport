using Airport.Data.Model;
using AirportBusinessLogic.Dtos;
using AirportBusinessLogic.Interfaces;
using AutoMapper;
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

        [HttpGet("GetAllFlights")]
        public async Task<ActionResult<IEnumerable<Flight>>> GetAllFlights()
        {
            var allFlights = await _businessService.GetAllFlights();
            return View(allFlights);
        }

        [Route("[action]", Name = "AddNewFlight")]
        [HttpPost]
        public async Task AddNewFlight(FlightCreateDto flight)
        {
            await _businessService.AddNewFlight(flight);
        }
    }
}
