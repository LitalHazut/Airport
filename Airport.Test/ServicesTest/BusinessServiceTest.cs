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
        FakeFlightRepository _fakeFlightRepository = new();
        FakeStationRepository _fakeStationRepository = new();
        FakeNextStationRepository _fakeNextStationRepository = new();
        FakeLiveUpdateRepository _fakeLiveUpdateRepository = new();
        FlightService _flightService;
        StationService _stationService;
        NextStationService _nextStation;
        LiveUpdateService _liveUpdate;
        BusinessService _businessService;
        private readonly IMapper _mapper;
        public BusinessServiceTest()
        {
            _flightService = new(_fakeFlightRepository);
            _stationService = new(_fakeStationRepository);
            _nextStation = new(_fakeNextStationRepository);
            _liveUpdate = new(_fakeLiveUpdateRepository);
            _businessService = new(_flightService, _stationService, _liveUpdate, _mapper, _nextStation);
        }
        [TestMethod]
        public async Task NextStationNotOccupy()
        {
            using (var context = new FakeAirportContext())
            {

                context.Flights.RemoveRange(context.Flights);
                context.Stations.RemoveRange(context.Stations);
                context.NextStations.RemoveRange(context.NextStations);
                context.LiveUpdates.RemoveRange(context.LiveUpdates);

                Flight flight1, flight2;
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
            using (var context = new FakeAirportContext())
            {
                var flightByStation = _stationService.Get(1)!.FlightId;
                var flightInStation = _flightService.Get((int)flightByStation!);
                Assert.IsFalse(await _businessService.MoveNextIfPossible(flightInStation!));
            }
        }

        [TestMethod]
        public async Task NextStationOccupy()
        {
            using (var context = new FakeAirportContext())
            {

                context.Flights.RemoveRange(context.Flights);
                context.Stations.RemoveRange(context.Stations);
                context.NextStations.RemoveRange(context.NextStations);
                context.LiveUpdates.RemoveRange(context.LiveUpdates);

                Flight flight1, flight2;
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

            using (var context = new FakeAirportContext())
            {
                var flightByStation = _stationService.Get(2)!.FlightId;
                var flightInStation = _flightService.Get((int)flightByStation!);
                Assert.IsTrue(await _businessService.MoveNextIfPossible(flightInStation!));
            }
        }
    }

}
