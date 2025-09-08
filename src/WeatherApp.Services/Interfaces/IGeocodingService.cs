using WeatherApp.Domain.ValueObjects;

namespace WeatherApp.Services.Interfaces;

/// <summary>
/// Service interface for geocoding operations.
/// </summary>
public interface IGeocodingService
{
    /// <summary>
    /// Gets the geographical coordinates (latitude and longitude) for a given zipcode.
    /// </summary>
    /// <param name="zipcode">The zipcode to geocode.</param>
    /// <returns>The geographical coordinates (latitude and longitude) for the given zipcode.</returns>
    Task<Coordinates> GetCoordinatesAsync(string zipcode);
}
