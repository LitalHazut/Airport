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
            _businessService= businessService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
