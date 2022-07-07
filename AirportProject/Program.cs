using Airport.Data.Contexts;
using Airport.Data.Model;
using Airport.Data.Repositories;
using Airport.Data.Repositories.Interfaces;
using AirportBusinessLogic.Interfaces;
using AirportBusinessLogic.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AirportContext>(opt =>
	opt.UseInMemoryDatabase("Airport")
);

builder.Services.AddScoped<IFlightRepository<Flight>, FlightRepository>();
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IStationRepository<Station>, StationRepository>();
builder.Services.AddScoped<IStationService, StationService>();
builder.Services.AddScoped<ILiveUpdateRepository<LiveUpdate>, LiveUpdateRepository>();
builder.Services.AddScoped<ILiveUpdateService, LiveUpdateService>();


builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger(); // Generate the JSON doc based on my code
	app.UseSwaggerUI(); // Expose a url for the json "/swagger"
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
