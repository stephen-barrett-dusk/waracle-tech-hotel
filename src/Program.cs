using hotel.Configuration;
using hotel.Service;
using hotel.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddSerilog(); // <-- Add this line

    var connection = builder.Configuration.GetConnectionString("hotel");

    builder.Services.AddDbContext<HotelContext>(options =>
        options.UseSqlServer(connection));

    // Add services to the container.
    builder.Services.AddScoped<IHotelService, HotelService>();
    builder.Services.AddScoped<IHotelRoomService, HotelRoomService>();
    builder.Services.AddScoped<IRoomBookingService, RoomBookingService>();
    builder.Services.AddScoped<ISeedingService, SeedingService>();

    builder.Services.AddControllers();
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    var app = builder.Build();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

