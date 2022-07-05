using Airport.Data.Model;
using AirportBusinessLogic.Interfaces;
using AirportBusinessLogic.Services;
using Moq;
namespace Business.Test
{
    [TestClass]
    public class StationTest
    {

        [TestMethod]
        public void TestQuantityOfStations()
        {
            var stationRepositoryMock = new Mock<IStationRepository>();
            var stationService = new StationService(stationRepositoryMock.Object);

            var stations = new List<Station>();
            stations.Add(new Station { StationId = 1, Name = "First Station", FlightId = 1, Flight = null });
            stations.Add(new Station { StationId = 2, Name = "Second Station", FlightId = 2, Flight = null });

            stationRepositoryMock.Setup(s => s.GetAll()).Returns(stations.AsQueryable());

            Assert.AreEqual(stationService.GetAll().Count(), 2);
        }
        public void MoveFlightToFromStation1ToStation2()
        {




           
        }
    }
}