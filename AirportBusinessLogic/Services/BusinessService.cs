using Airport.Data.Model;
using AirportBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportBusinessLogic.Services
{
    public class BusinessService: IBusinessService
    {
        private readonly IFlightService<Flight> _flightService;
        private readonly IStationService _stationService;
        private readonly ILiveUpdateService _liveUpdateService;
        public BusinessService(IFlightService<Flight> flightService, IStationService stationService, ILiveUpdateService liveUpdateService)
        {
            _flightService=flightService;
            _stationService=stationService; 
            _liveUpdateService=liveUpdateService;   
        }
    }
}
