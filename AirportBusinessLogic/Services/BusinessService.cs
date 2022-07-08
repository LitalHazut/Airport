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
        private readonly IStationService<Station> _stationService;
        private readonly ILiveUpdateService<LiveUpdate> _liveUpdateService;
        public BusinessService(IFlightService<Flight> flightService, IStationService<Station> stationService, ILiveUpdateService<LiveUpdate> liveUpdateService)
        {
            _flightService=flightService;
            _stationService=stationService; 
            _liveUpdateService=liveUpdateService;   
        }
    }
}
