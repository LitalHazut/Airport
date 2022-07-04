
using Airport.Data.Model;
using AirportBusinessLogic.Services;
using Moq;

namespace Business.Test
{
    [TestClass]
    public class StationTest
    {

        StationService station = new StationService();

        [TestMethod]
        public void TestQuantityOfStations()
        {
            //var stationRepositoryMock = new Mock<IStationRepository<Station>>();
            Assert.IsTrue(station.GetAll().Count() == 8);

        }
        public void TestAllConnectionsBetweenStations()
        {
          //check s - 1 s -2 
          // s 2- s 3
          //s 3- s 4
          //s 4- s 5
          //s 5- s 6
          //s 5- s 7
          //s 6- s 8
          //s 7- s 8
          //s 8- s 4
        }
    }
}