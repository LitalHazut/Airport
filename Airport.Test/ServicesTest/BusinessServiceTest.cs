using Airport.Data.Model;
using AirportBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport.Test.ServicesTest
{
    [TestClass]
    public class BusinessServiceTest
    {

        private IFlightService _flightService;
        public BusinessServiceTest(IFlightService flightService)
        {
            _flightService = flightService;
        }
            

        [TestMethod]

        public void StationNotOccupy()
        {
            

        }


    }
}
