using Airport.Client.Helper;
using Airport.Client.Models;
using AirportBusinessLogic.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Airport.Client.Controllers
{
    public class HomeController : Controller
    {
        AirportApi _api = new AirportApi();
        public async Task<IActionResult> GetPendingFlightsByAsc(bool isAsc)
        {
            List<FlightReadDto> flightList = new List<FlightReadDto>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync($"api/Airport/GetPendingFlightsByAsc/{isAsc}");
            if (res.IsSuccessStatusCode)
            {
                var result= res.Content.ReadAsStringAsync().Result;
                flightList = JsonConvert.DeserializeObject<List<FlightReadDto>>(result);
            }

            return View(flightList);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}