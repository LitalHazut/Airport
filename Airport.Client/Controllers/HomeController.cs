﻿using Airport.Client.Helper;
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
            List<Station> stationList = new List<Station>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/Airport/GetAllStationsStatus");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                stationList = JsonConvert.DeserializeObject<List<Station>>(result)!;
               
            }

            return Json(new { data = stationList });
        }
           
        public async Task<IActionResult> SeeAllLiveUpdates()
        {
            List<LiveUpdate> liveUpdates = new List<LiveUpdate>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/Airport/SeeAllLiveUpdates");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                liveUpdates = JsonConvert.DeserializeObject<List<LiveUpdate>>(result)!;
            }
            return View(liveUpdates);
        }

        public async Task<IActionResult> GetAllStationsStatus()
        {
            List<Station> station = new List<Station>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/Airport/GetAllStationsStatus");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                station = JsonConvert.DeserializeObject<List<Station>>(result)!;
            }
            return View(station);
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