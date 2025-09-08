using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Repositories;
using WeatherApp.Domain.ValueObjects;
using WeatherApp.Infrastructure.Models;

namespace WeatherApp.Infrastructure.Repositories;

/// <summary>
/// Cache wrapper for weather repository.
/// </summary>
public class CachedWeatherRepository : IWeatherRepository
{
    private readonly IWeatherRepository _innerRepository;
    private readonly IMemoryCache _cache;
    private readonly ApiOptions _options;
    private readonly TimeSpan _cacheDuration;

    public CachedWeatherRepository(IWeatherRepository innerRepository, IMemoryCache cache, IOptionsSnapshot<ApiOptions> options)
    {
        _innerRepository = innerRepository ?? throw new ArgumentNullException(nameof(innerRepository));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _options = options.Get("WeatherApi") ?? throw new ArgumentNullException(nameof(options));
        _cacheDuration = TimeSpan.FromMinutes(_options.CacheTTLMinutes);
    }

    public async Task<WeatherForecast> GetCurrentWeather(Coordinates coordinates)
    {
        string cacheKey = $"weather:current:{coordinates.Latitude}:{coordinates.Longitude}";

        if (_cache.TryGetValue(cacheKey, out WeatherForecast? forecast) && forecast != null)
            return forecast;

        forecast = await _innerRepository.GetCurrentWeather(coordinates);
        _cache.Set(cacheKey, forecast, _cacheDuration);
        return forecast;
    }

    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast(Coordinates coordinates, int days)
    {
        string cacheKey = $"weather:forecast:{coordinates.Latitude}:{coordinates.Longitude}:{days}";

        if (_cache.TryGetValue(cacheKey, out IEnumerable<WeatherForecast>? forecast) && forecast != null)
            return forecast;

        forecast = await _innerRepository.GetWeatherForecast(coordinates, days);
        _cache.Set(cacheKey, forecast, _cacheDuration);
        return forecast;
    }
}
