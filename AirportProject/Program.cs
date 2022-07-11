using Airport.Data.Contexts;
using Airport.Data.Model;
using Airport.Data.Repositories;
using Airport.Data.Repositories.Interfaces;
using AirportBusinessLogic.Interfaces;
using AirportBusinessLogic.Profiles;
using AirportBusinessLogic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AirportContext>(opt =>
    opt.UseInMemoryDatabase("Airport")
);

builder.Services.AddScoped<IFlightRepository<Flight>, FlightRepository>();
builder.Services.AddScoped<IFlightService<Flight>, FlightService>();
builder.Services.AddScoped<INextStationRepository<NextStation>, NextStationRepository>();
builder.Services.AddScoped<IStationRepository<Station>, StationRepository>();
builder.Services.AddScoped<IStationService<Station>, StationService>();
builder.Services.AddScoped<INextStationService<NextStation>, NextStationService>();
builder.Services.AddScoped<ILiveUpdateRepository<LiveUpdate>, LiveUpdateRepository>();
builder.Services.AddScoped<ILiveUpdateService<LiveUpdate>, LiveUpdateService>();
builder.Services.AddScoped<IBusinessService, BusinessService>();

builder.Services.AddAutoMapper(typeof(FlightsProfile).Assembly);

builder.Services.AddCors(options =>
{
    options.AddPolicy("myPolicy",
                      policy =>
                      {
                          policy
                            .WithOrigins("https://localhost:7237")
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                      });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Airport API",

    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger(); // Generate the JSON doc based on my code
	app.UseSwaggerUI(); // Expose a url for the json "/swagger"
}

app.UseHttpsRedirection();

app.UseCors("myPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
