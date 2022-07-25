using Airport.Data.Contexts;
using Airport.Data.Model;
using Airport.Data.Repositories;
using Airport.Data.Repositories.Interfaces;
using Airport.Test.FakeContext;
using Airport.Test.FakeReopositories;
using AirportBusinessLogic.Interfaces;
using AirportBusinessLogic.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Airport.Test.ServicesTest
{
    [TestClass]
    public class BusinessServiceTest
    {
        private IBusinessService _IbusinessService;
        FakeFlightRepository _fakeFlightRepository = new();
        FakeStationRepository _fakeStationRepository = new();
        FakeNextStationRepository _fakeNextStationRepository = new();
        FakeLiveUpdateRepository _fakeLiveUpdateRepository= new();
        FlightService _flightService;
        StationService _stationService;
        NextStationService _nextStation;
        LiveUpdateService _liveUpdate;
        BusinessService _businessService;
        private readonly IMapper _mapper;
        public BusinessServiceTest(IBusinessService businessService)
        {
            _IbusinessService = businessService;

            _flightService = new(_fakeFlightRepository);
            _stationService = new(_fakeStationRepository);
            _nextStation = new(_fakeNextStationRepository);
            _liveUpdate = new(_fakeLiveUpdateRepository);
            
            _businessService = new(_flightService, _stationService, _liveUpdate, _mapper, _nextStation);

        }
        public BusinessServiceTest()
        {

        }

        private static void LoadData(FakeAirportContext context, out Flight flight1, out Flight flight2)
        {
            flight1 = new Flight() { IsAscending = true, IsDone = true, IsPending = true, InsertionTime = DateTime.Now, TimerFinished = false };
            flight2 = new Flight() { IsAscending = true, IsDone = true, IsPending = true, InsertionTime = DateTime.Now, TimerFinished = false };
            context.Flights.Add(flight1);
            context.Flights.Add(flight2);

            context.SaveChanges();

            context.Stations.Add(new() { StationNumber = 1, FlightId = flight1.FlightId });
            context.Stations.Add(new() { StationNumber = 2, FlightId = flight2.FlightId });
            context.Stations.Add(new() { StationNumber = 3, FlightId = null });
            context.Stations.Add(new() { StationNumber = 4, FlightId = null });

            context.SaveChanges();

            context.NextStations.Add(new() { SourceId = null, FlightType = true, TargetId = 1 });
            context.NextStations.Add(new() { SourceId = 2, FlightType = true, TargetId = 3 });
            context.NextStations.Add(new() { SourceId = 3, FlightType = true, TargetId = 4 });
            context.SaveChanges();
        }

        [TestMethod]
        public void StationNotOccupy()
        {
            using (var context = new FakeAirportContext())
            {
                Flight flight1, flight2;
                LoadData(context, out flight1, out flight2);

                Assert.IsFalse(_IbusinessService.MoveNextIfPossible(flight1).Result);
            }
        }

      
        [TestMethod]
        public void StationOccupy()
        {
            using (var context = new FakeAirportContext())
            {
                Flight flight1, flight2;
                LoadData(context, out flight1, out flight2);

                Assert.IsTrue(_IbusinessService.MoveNextIfPossible(flight2).Result);
            }
        }
    }

}
