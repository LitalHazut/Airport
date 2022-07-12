using Airport.Data.Model;
using AirportBusinessLogic.Interfaces;
using AirportBusinessLogic.Services;
using Moq;
namespace Business.Test
{
    [TestClass]
    public class StationTest
    {

        //[TestMethod]
        //public void TestQuantityOfStations()
        //{
        //    var stationRepositoryMock = new Mock<IStationRepository>();
        //    var stationService = new StationService(stationRepositoryMock.Object);

        //    var stations = new List<Station>();
        //    stations.Add(new Station { StationId = 1,FlightId=1});
        //    stations.Add(new Station { StationId = 2, FlightId = 2 });

        //    stationRepositoryMock.Setup(s => s.GetAll()).Returns(stations.AsQueryable());

        //    Assert.AreEqual(stationService.GetAll().Count(), 2);
        //}

        //public void MoveFlightFromStation1ToStation2()
        //{
        //    var stationRepositoryMock = new Mock<IStationRepository>();
        //    var stationService = new StationService(stationRepositoryMock.Object);

        //    var stations = new List<Station>();
        //    var station1 = new Station { StationId = 1,  FlightId = 1 };
        //    var station2 = new Station { StationId = 2,  FlightId = 2 };
        //    stations.Add(station1);
        //    stations.Add(station2);

        //    var flight = new Flight { FlightId = 3 };
        //}
    }
}