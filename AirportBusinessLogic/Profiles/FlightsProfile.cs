using Airport.Data.Model;
using AirportBusinessLogic.Dtos;
using AutoMapper;

namespace AirportBusinessLogic.Profiles
{
    public class FlightsProfile : Profile
    {
        public FlightsProfile()
        {
            CreateMap<Flight, FlightReadDto>();
            CreateMap<FlightCreateDto, Flight>();
        }
    }
}
