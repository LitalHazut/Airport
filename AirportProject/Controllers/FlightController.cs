using Microsoft.AspNetCore.Mvc;

namespace AirportProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
