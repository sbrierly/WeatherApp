using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using WeatherApp.Domain.ValueObjects;
using WeatherApp.Infrastructure.Models;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Infrastructure.Geo;

/// <summary>
/// Cache wrapper for geocoding service.
/// </summary>
public class CachedGeocodingService : IGeocodingService
{
    private readonly IGeocodingService _innerService;
    private readonly IMemoryCache _cache;
    private readonly ApiOptions _options;
    private TimeSpan _cacheDuration;

    public CachedGeocodingService(IGeocodingService innerService, IMemoryCache cache, IOptionsSnapshot<ApiOptions> options)
    {
        _innerService = innerService ?? throw new ArgumentNullException(nameof(innerService));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _options = options.Get("GeoApi") ?? throw new ArgumentNullException(nameof(options));
        _cacheDuration = TimeSpan.FromMinutes(_options.CacheTTLMinutes);
    }

    public async Task<Coordinates> GetCoordinatesAsync(string zipcode)
    {
        string cacheKey = $"geocoding:{zipcode}";

        if (_cache.TryGetValue(cacheKey, out Coordinates coordinates))
            return coordinates;

        coordinates = await _innerService.GetCoordinatesAsync(zipcode);
        _cache.Set(cacheKey, coordinates, _cacheDuration);
        return coordinates;
    }
}
