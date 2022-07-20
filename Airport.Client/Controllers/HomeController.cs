using Airport.Client.Helper;
using Airport.Client.Models;
using Airport.Data.Model;
using AirportBusinessLogic.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;


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
                var result = res.Content.ReadAsStringAsync().Result;
                flightList = JsonConvert.DeserializeObject<List<FlightReadDto>>(result)!;
            }

            return View(flightList);
        }

        public async Task<IActionResult> AddNewFlight(bool isAsc)
        {
            using (var client = _api.Initial())
            {
                var endpoint = "api/Airport/AddNewFlight";
                var flight = new FlightCreateDto()
                {
                    IsAscending = isAsc
                };

                var newFlight = JsonConvert.SerializeObject(flight);
                var payload = new StringContent(newFlight, Encoding.UTF8, "application/json");
                var result = client.PostAsync(endpoint, payload).Result.Content.ReadAsStringAsync().Result;
            }

            return RedirectToAction("GetAllStationsStatus", "Home");
        }
        
        public async Task<JsonResult> LoadStations()
        {
            List<StationStatus> stationStatusList = new();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/Airport/GetStationsStatusList");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                stationStatusList = JsonConvert.DeserializeObject<List<StationStatus>>(result)!;
            }

            return Json(new { stationStatusList = stationStatusList });
        }

        public async Task<IActionResult> SeeAllLiveUpdates(int pageNum=1)
        {
            List<LiveUpdate> liveUpdates = new List<LiveUpdate>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/Airport/SeeAllLiveUpdates");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                liveUpdates = JsonConvert.DeserializeObject<List<LiveUpdate>>(result)!;
            }

            liveUpdates.Reverse();
            var elementCountForPage = 15;
            var startingIndex = (pageNum - 1) * elementCountForPage;
            var count= Math.Min(elementCountForPage, liveUpdates.Count-startingIndex);
            var pageList = liveUpdates.GetRange(startingIndex, count);
            var lastPage = liveUpdates.Count / elementCountForPage;
            if(liveUpdates.Count % elementCountForPage != 0)
            {
                lastPage++;
            }
            LiveUpdateViewModel viewModel = new()
            {
                IsFirstPage = pageNum == 1,
                IsLastPage = pageNum == lastPage,
                LiveUpdateList = pageList,
                CurrentPage = pageNum
            };
            return View(viewModel);
        }

        public async Task<IActionResult> GetAllStationsStatus()
        {
            List<StationStatus> stationList = new();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/Airport/GetAllStationsStatus");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                stationList = JsonConvert.DeserializeObject<List<StationStatus>>(result)!;
            }
            return View(stationList);
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