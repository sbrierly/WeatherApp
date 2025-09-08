using System.Text.Json.Serialization;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;



using WeatherApp.Domain.Repositories;
using WeatherApp.Domain.Services;
using WeatherApp.Infrastructure.Geo;
using WeatherApp.Infrastructure.Models;
using WeatherApp.Infrastructure.Repositories;
using WeatherApp.Services.Implementations;
using WeatherApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IWeatherAnalysisService, WeatherAnalysisService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();

builder.Services.Configure<ApiOptions>("WeatherApi", builder.Configuration.GetSection("WeatherApi"));
builder.Services.AddHttpClient<IWeatherRepository, OpenWeatherRepository>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<OpenWeatherRepository>();
builder.Services.AddScoped<IWeatherRepository>(sp =>
{
    var inner = sp.GetRequiredService<OpenWeatherRepository>();
    var cache = sp.GetRequiredService<IMemoryCache>();
    var options = sp.GetRequiredService<IOptionsSnapshot<ApiOptions>>();
    return new CachedWeatherRepository(inner, cache, options);
});

builder.Services.Configure<ApiOptions>("GeoApi", builder.Configuration.GetSection("GeoApi"));
builder.Services.AddHttpClient<IGeocodingService, GeocodingService>();
builder.Services.AddScoped<GeocodingService>();
builder.Services.AddScoped<IGeocodingService>(sp =>
{
    var inner = sp.GetRequiredService<GeocodingService>();
    var cache = sp.GetRequiredService<IMemoryCache>();
    var options = sp.GetRequiredService<IOptionsSnapshot<ApiOptions>>();
    return new CachedGeocodingService(inner, cache, options);
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
