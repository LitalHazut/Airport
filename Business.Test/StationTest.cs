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
            stationRepositoryMock.SetupAllProperties();
            stationRepositoryMock.SetupGet(s => s.GetAll().Count()).Returns(8);

            Action act = () =>
            {
                var station = new StationService(stationRepositoryMock.Object);
                Assert.AreEqual(8, station.GetAll().Count());


            };
            
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