using System.Net.Http.Json;

using Microsoft.Extensions.Options;

using WeatherApp.Domain.ValueObjects;
using WeatherApp.Infrastructure.Models;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Infrastructure.Geo;

/// <summary>
/// Service for geocoding operations using an external API.
/// </summary>
public class GeocodingService : IGeocodingService
{
    private readonly HttpClient _httpClient;
    private readonly ApiOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="GeocodingService"/> class.
    /// </summary>
    /// <param name="httpClient">HttpClient for making API requests.</param>
    /// <param name="options">Options for the geocoding API.</param>
    public GeocodingService(HttpClient httpClient, IOptionsSnapshot<ApiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Get("GeoApi");
    }

    /// <summary>
    /// Gets the geographical coordinates (latitude and longitude) for a given ZIP code.
    /// </summary>
    /// <param name="zipcode">ZIP code to get coordinates for.</param>
    /// <returns>Coordinates for the given ZIP code.</returns>
    /// <exception cref="ZipcodeNotFoundException"></exception>
    public async Task<Coordinates> GetCoordinatesAsync(string zipcode)
    {
        var url = $"{_options.BaseUrl}?zip={zipcode}&appid={_options.ApiKey}";

        var response = await _httpClient.GetAsync(url);
        var data = await response.Content.ReadFromJsonAsync<GeocodingResult>();

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            throw new ZipcodeNotFoundException(zipcode);
        else if (!response.IsSuccessStatusCode || data == null)
            throw new InvalidOperationException($"Failed to retrieve coordinates for ZIP code '{zipcode}'.");
        else
            return new Coordinates(data.Latitude, data.Longitude);
    }
}
