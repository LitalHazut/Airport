using AirportBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportBusinessLogic.Services
{
    public class FlightService:IFlightService
    {
        private readonly IFlightService _flightService;
        public FlightService(IFlightService flightService)
        {
            _flightService = flightService;
        }

    }
}
