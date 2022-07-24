using Airport.Data.Model;

namespace AirportBusinessLogic.Interfaces
{
    public interface IFlightService: ICRUDService<Flight>
    {
        Flight? GetFirstFlightInQueue(List<Station> pointingStations, bool? isFirstAscendingStation,bool isFiveOccupied);

    }
}
