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

    /// <summary>
    /// Cached Geocoding Service.
    /// </summary>
    /// <param name="innerService">The IGeocodingService service for which values will be cached.</param>
    /// <param name="cache">The type of cache to use.</param>
    /// <param name="options">Options for the API.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public CachedGeocodingService(IGeocodingService innerService, IMemoryCache cache, IOptionsSnapshot<ApiOptions> options)
    {
        _innerService = innerService ?? throw new ArgumentNullException(nameof(innerService));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _options = options.Get("GeoApi") ?? throw new ArgumentNullException(nameof(options));
        _cacheDuration = TimeSpan.FromMinutes(_options.CacheTTLMinutes);
    }

    /// <summary>
    /// Get coordinates from cache, if available, otherwise call inner service.
    /// </summary>
    /// <param name="zipcode"></param>
    /// <returns></returns>
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
