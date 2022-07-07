using AirportBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportBusinessLogic.Services
{
    public class StationService:IStationService
    {
        private readonly IStationService _stationService;
        public StationService(IStationService stationService)
        {
            _stationService = stationService;
        }
    }
}
