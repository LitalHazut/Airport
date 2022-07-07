using Airport.Data.Contexts;
using Airport.Data.Model;
using Airport.Data.Repositories;
using Airport.Data.Repositories.Interfaces;
using AirportBusinessLogic.Interfaces;
using AirportBusinessLogic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AirportContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("AirportDataConnection")));

builder.Services.AddScoped<IFlightRepository<Flight>, FlightRepository>();
builder.Services.AddScoped<IFlightService<Flight>, FlightService>();
builder.Services.AddScoped<IStationRepository<Station>, StationRepository>();
builder.Services.AddScoped<IStationService, StationService>();
builder.Services.AddScoped<ILiveUpdateRepository<LiveUpdate>, LiveUpdateRepository>();
builder.Services.AddScoped<ILiveUpdateService, LiveUpdateService>();
builder.Services.AddScoped<IBusinessService, BusinessService>();

builder.Services.AddCors(options => {
    options.AddPolicy("myPolicy",
                      policy => {
                          policy
                            .WithOrigins("https://localhost:7237")
                            //.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                      });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo
    {

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
