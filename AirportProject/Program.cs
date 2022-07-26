using Airport.Data.Contexts;
using Airport.Data.Repositories;
using Airport.Data.Repositories.Interfaces;
using AirportBusinessLogic.Interfaces;
using AirportBusinessLogic.Profiles;
using AirportBusinessLogic.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AirportContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AirPortDataConnectionString"));
}
, ServiceLifetime.Transient);
builder.Services.AddControllers();

builder.Services.AddTransient<IFlightRepository, FlightRepository>();
builder.Services.AddTransient<INextStationRepository, NextStationRepository>();
builder.Services.AddTransient<IStationRepository, StationRepository>();
builder.Services.AddTransient<ILiveUpdateRepository, LiveUpdateRepository>();
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IStationService, StationService>();
builder.Services.AddScoped<INextStationService, NextStationService>();
builder.Services.AddScoped<ILiveUpdateService, LiveUpdateService>();
builder.Services.AddScoped<IBusinessService, BusinessService>();
builder.Services.AddRouting();
builder.Services.AddAutoMapper(typeof(FlightsProfile).Assembly);

builder.Services.AddCors(options => {
    options.AddPolicy("myPolicy",
                      policy => {
                          policy
                            .WithOrigins("https://localhost:5042")
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                      });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
