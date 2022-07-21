using Airport.Data.Contexts;
using Airport.Data.Model;
using Airport.Data.Repositories;
using Airport.Data.Repositories.Interfaces;
using AirportBusinessLogic.Interfaces;
using AirportBusinessLogic.Profiles;
using AirportBusinessLogic.Services;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AirportContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AirPortDataConnectionString"));
}
, ServiceLifetime.Transient);
builder.Services.AddControllers();

builder.Services.AddTransient<IFlightRepository<Flight>, FlightRepository>();
builder.Services.AddTransient<INextStationRepository<NextStation>, NextStationRepository>();
builder.Services.AddTransient<IStationRepository<Station>, StationRepository>();
builder.Services.AddTransient<ILiveUpdateRepository<LiveUpdate>, LiveUpdateRepository>();
builder.Services.AddScoped<IFlightService<Flight>, FlightService>();
builder.Services.AddScoped<IStationService<Station>, StationService>();
builder.Services.AddScoped<INextStationService<NextStation>, NextStationService>();
builder.Services.AddScoped<ILiveUpdateService<LiveUpdate>, LiveUpdateService>();
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

var connectionString = builder.Configuration.GetConnectionString("AirPortDataConnectionString");
builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        }));
builder.Services.AddHangfireServer();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger(); // Generate the JSON doc based on my code
	app.UseSwaggerUI(); // Expose a url for the json "/swagger"
}
app.UseHangfireDashboard();

app.UseHttpsRedirection();

app.UseCors("myPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
