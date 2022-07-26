using Airport.Data.Contexts;
using Airport.Data.Model;
using Airport.Data.Repositories;
using Airport.Data.Repositories.Interfaces;
using Airport.Test.FakeContext;
using Airport.Test.FakeReopositories;
using AirportBusinessLogic.Dtos;
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
        public async Task NoFlightAdded()
        {
            using (var context = new FakeAirportContext())
            {
                context.Flights.RemoveRange(context.Flights);
                context.Stations.RemoveRange(context.Stations);
                context.NextStations.RemoveRange(context.NextStations);
                context.LiveUpdates.RemoveRange(context.LiveUpdates);

                Flight flight1;
                Flight flight2;

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
                context.NextStations.Add(new() { SourceId = 1, FlightType = true, TargetId = 2 });
                context.NextStations.Add(new() { SourceId = 2, FlightType = true, TargetId = 3 });
                context.NextStations.Add(new() { SourceId = 3, FlightType = true, TargetId = 4 });
                context.NextStations.Add(new() { SourceId = 4, FlightType = true, TargetId = null });

                context.SaveChanges();
            }

            using (var context = new FakeAirportContext())
            {
                if (_fakeFlightRepository.GetAll().ToList().Count() == 0)
                {
                    var flightDto = new FlightCreateDto() { IsAscending = true };
                    await _businessService.AddNewFlight(flightDto);
                    var flight2 = _fakeFlightRepository.GetAll().ToList().First();
                    Assert.IsTrue(flight2.IsPending);
                }
            }
        }

        [TestMethod]
        public async Task NextStationIsNotOccupy()
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
                context.NextStations.Add(new() { SourceId = 1, FlightType = true, TargetId =2 });
                context.NextStations.Add(new() { SourceId = 2, FlightType = true, TargetId = 3 });
                context.NextStations.Add(new() { SourceId = 3, FlightType = true, TargetId = 4 });
                context.NextStations.Add(new() { SourceId = 4, FlightType = true, TargetId = null });

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
        public async Task NextStationIsOccupy()
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
                context.NextStations.Add(new() { SourceId = 1, FlightType = true, TargetId = 2 });
                context.NextStations.Add(new() { SourceId = 2, FlightType = true, TargetId = 3 });
                context.NextStations.Add(new() { SourceId = 3, FlightType = true, TargetId = 4 });
                context.NextStations.Add(new() { SourceId = 4, FlightType = true, TargetId = null });

                context.SaveChanges();
            }

            using (var context = new FakeAirportContext())
            {
                var flightByStation = _stationService.Get(2)!.FlightId;
                var flightInStation = _flightService.Get((int)flightByStation!);
                Assert.IsTrue(await _businessService.MoveNextIfPossible(flightInStation!));
            }
        }

        [TestMethod]
        public async Task PreviousStationInEmpty()
        {
            using (var context = new FakeAirportContext())
            {
                context.Flights.RemoveRange(context.Flights);
                context.Stations.RemoveRange(context.Stations);
                context.NextStations.RemoveRange(context.NextStations);
                context.LiveUpdates.RemoveRange(context.LiveUpdates);

                Flight flight1;
                flight1 = new Flight() { IsAscending = true, IsDone = false, IsPending = false, InsertionTime = DateTime.Now, TimerFinished = false };
                context.Flights.Add(flight1);
               

                context.SaveChanges();

                context.Stations.Add(new() { StationNumber = 1, FlightId = flight1.FlightId });
                context.Stations.Add(new() { StationNumber = 2, FlightId = null });
                context.Stations.Add(new() { StationNumber = 3, FlightId = null });
                context.Stations.Add(new() { StationNumber = 4, FlightId = null });

                context.SaveChanges();


                context.NextStations.Add(new() { SourceId = null, FlightType = true, TargetId = 1 });
                context.NextStations.Add(new() { SourceId = 1, FlightType = true, TargetId = 2 });
                context.NextStations.Add(new() { SourceId = 2, FlightType = true, TargetId = 3 });
                context.NextStations.Add(new() { SourceId = 3, FlightType = true, TargetId = 4 });
                context.NextStations.Add(new() { SourceId = 4, FlightType = true, TargetId = null });

                context.SaveChanges();
            }
            using (var context = new FakeAirportContext())
            {
                var flightByStation = _stationService.Get(1).FlightId;
                var flightInStation = _flightService.Get((int)flightByStation!);
                await _businessService.MoveNextIfPossible(flightInStation);
                Assert.IsNull(_stationService.Get(1).FlightId) ;
            }
        }

        [TestMethod]
        public async Task FlightTimerFinished()
        {
            using (var context = new FakeAirportContext())
            {
                context.Flights.RemoveRange(context.Flights);
                context.Stations.RemoveRange(context.Stations);
                context.NextStations.RemoveRange(context.NextStations);
                context.LiveUpdates.RemoveRange(context.LiveUpdates);

                Flight flight1;
                flight1 = new Flight() { IsAscending = true, IsDone = false, IsPending = false, InsertionTime = DateTime.Now, TimerFinished = false };
                context.Flights.Add(flight1);


                context.SaveChanges();

                context.Stations.Add(new() { StationNumber = 1, FlightId = null });
                context.Stations.Add(new() { StationNumber = 2, FlightId = null });
                context.Stations.Add(new() { StationNumber = 3, FlightId = null });
                context.Stations.Add(new() { StationNumber = 4, FlightId = flight1.FlightId });

                context.SaveChanges();


                context.NextStations.Add(new() { SourceId = null, FlightType = true, TargetId = 1 });
                context.NextStations.Add(new() { SourceId = 1, FlightType = true, TargetId = 2 });
                context.NextStations.Add(new() { SourceId = 2, FlightType = true, TargetId = 3 });
                context.NextStations.Add(new() { SourceId = 3, FlightType = true, TargetId = 4 });
                context.NextStations.Add(new() { SourceId = 4, FlightType = true, TargetId = null });
                context.SaveChanges();
            }
            using (var context = new FakeAirportContext())
            {
                var flightByStation = _stationService.Get(4).FlightId;
                var flightInStation = _flightService.Get((int)flightByStation!);
                await _businessService.MoveNextIfPossible(flightInStation!);
                Assert.IsNull(_stationService.Get(4).FlightId);
            }
        }

        [TestMethod]
        public async Task TestIfFlightStuckInStation()
        {
            Flight flight2;
            using (var context = new FakeAirportContext())
            {
                context.Flights.RemoveRange(context.Flights);
                context.Stations.RemoveRange(context.Stations);
                context.NextStations.RemoveRange(context.NextStations);
                context.LiveUpdates.RemoveRange(context.LiveUpdates);

                var flight1 = new Flight() { IsAscending = true, IsDone = false, IsPending = false, InsertionTime = DateTime.Now, TimerFinished = false };
                flight2 = new Flight() { IsAscending = true, IsDone = false, IsPending = true, InsertionTime = DateTime.Now, TimerFinished = false };

                context.Flights.Add(flight1);
                context.Flights.Add(flight2);

                context.SaveChanges();

                context.Stations.Add(new() { StationNumber = 1, FlightId = null });
                context.Stations.Add(new() { StationNumber = 2, FlightId = null });
                context.Stations.Add(new() { StationNumber = 3, FlightId = flight1.FlightId });
                context.Stations.Add(new() { StationNumber = 4, FlightId = null });

                context.SaveChanges();


                context.NextStations.Add(new() { SourceId = null, FlightType = true, TargetId = 1 });
                context.NextStations.Add(new() { SourceId = 1, FlightType = true, TargetId = 2 });
                context.NextStations.Add(new() { SourceId = 2, FlightType = true, TargetId = 3 });
                context.NextStations.Add(new() { SourceId = 3, FlightType = true, TargetId = 4 });
                context.NextStations.Add(new() { SourceId = 4, FlightType = true, TargetId = null });
                context.SaveChanges();
            }
            using (var context = new FakeAirportContext())
            {
                await _businessService.MoveNextIfPossible(flight2);

                Assert.IsTrue(flight2.TimerFinished);
                Assert.IsTrue(_stationService.Get(2)!.FlightId == flight2.FlightId);
            }
        }

        [TestMethod]
        public async Task RoutesOfFlightByIsAscending()
        {
            using (var context = new FakeAirportContext())
            {
                context.Flights.RemoveRange(context.Flights);
                context.Stations.RemoveRange(context.Stations);
                context.NextStations.RemoveRange(context.NextStations);
                context.LiveUpdates.RemoveRange(context.LiveUpdates);

                Flight flight1, flight2;
                flight1 = new Flight() { IsAscending = false, IsDone = false, IsPending = false, InsertionTime = DateTime.Now, TimerFinished = false };
                flight2 = new Flight() { IsAscending = true, IsDone = false, IsPending = false, InsertionTime = DateTime.Now, TimerFinished = false };

                context.Flights.Add(flight1);
                context.Flights.Add(flight2);

                context.SaveChanges();

                context.Stations.Add(new() { StationNumber = 1, FlightId = flight1.FlightId });
                context.Stations.Add(new() { StationNumber = 2, FlightId = null });
                context.Stations.Add(new() { StationNumber = 5, FlightId = null });
                context.Stations.Add(new() { StationNumber = 6, FlightId = flight2.FlightId }); 
                context.Stations.Add(new() { StationNumber = 8, FlightId = null }); 

                context.SaveChanges();

                context.NextStations.Add(new() { SourceId = null, FlightType = true, TargetId = 1 });
                context.NextStations.Add(new() { SourceId = 1, FlightType = true, TargetId = 2 });
                context.NextStations.Add(new() { SourceId = 2, FlightType = true, TargetId = 3 });
                context.NextStations.Add(new() { SourceId = 6, FlightType = true, TargetId = null});
                context.NextStations.Add(new() { SourceId = 6, FlightType = false, TargetId = 8});
                context.SaveChanges();
            }
            using (var context = new FakeAirportContext())
            {
                var flight1 = _stationService.Get(1)!.FlightId;
                var flight2 = _stationService.Get(6)!.FlightId;
                var flightInStation1 = _flightService.Get((int)flight1!);
                var flightInStation2 = _flightService.Get((int)flight2!);
                await _businessService.MoveNextIfPossible(flightInStation1!);
                await _businessService.MoveNextIfPossible(flightInStation2!);

                Assert.IsTrue(flightInStation1!.IsAscending == false);
                Assert.IsTrue(flightInStation2!.IsAscending == true);


            }
        }

        [TestMethod]
        public async Task TakeoffNotMoveToNextStation()
        {
            using (var context = new FakeAirportContext())
            {
                context.Flights.RemoveRange(context.Flights);
                context.Stations.RemoveRange(context.Stations);
                context.NextStations.RemoveRange(context.NextStations);
                context.LiveUpdates.RemoveRange(context.LiveUpdates);

                Flight flight1, flight2;
                flight1 = new Flight() { IsAscending = true, IsDone = false, IsPending = false, InsertionTime = DateTime.Now, TimerFinished = false };
                flight2 = new Flight() { IsAscending = true, IsDone = false, IsPending = false, InsertionTime = DateTime.Now, TimerFinished = false };

                context.Flights.Add(flight1);
                context.Flights.Add(flight2);

                context.SaveChanges();

                context.Stations.Add(new() { StationNumber = 1, FlightId = flight1.FlightId });
                context.Stations.Add(new() { StationNumber = 2, FlightId = flight2.FlightId });
                context.Stations.Add(new() { StationNumber = 3, FlightId = null });
              

                context.SaveChanges();

                context.NextStations.Add(new() { SourceId = null, FlightType = true, TargetId = 1 });
                context.NextStations.Add(new() { SourceId = 1, FlightType = true, TargetId = 2 });
                context.NextStations.Add(new() { SourceId = 2, FlightType = true, TargetId = null });
                context.NextStations.Add(new() { SourceId = 1, FlightType = false, TargetId = 3 });
                context.NextStations.Add(new() { SourceId = 3, FlightType = false, TargetId = null });
                context.NextStations.Add(new() { SourceId =null, FlightType = false, TargetId = 1 });
                context.SaveChanges();
            }
            using (var context = new FakeAirportContext())
            {
                var flight1 = _stationService.Get(1)!.FlightId;
                var flightInStation1 = _flightService.Get((int)flight1!);
                await _businessService.MoveNextIfPossible(flightInStation1!);
                Assert.IsTrue(_stationService.Get(1)!.FlightId == flightInStation1!.FlightId);
            }
        }
    }

}
