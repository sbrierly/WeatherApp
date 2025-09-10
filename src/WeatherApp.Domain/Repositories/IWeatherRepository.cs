namespace WeatherApp.Domain.Repositories;

using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Enums;
using WeatherApp.Domain.ValueObjects;

/// <summary>
/// Repository interface for fetching weather data from external sources.
/// </summary>
public interface IWeatherRepository
{
    /// <summary>
    /// Fetches current weather for a given coordinates.
    /// </summary>
    /// <param name="coordinates">The coordinates for the weather forecast.</param>
    /// <param name="units">The units for temperature (e.g., "fahrenheit" or "celsius"). Default is "fahrenheit".</param>
    /// <returns>Weather report for the location.</returns>
    Task<WeatherForecast> GetCurrentWeather(Coordinates coordinates);

    /// <summary>
    /// Fetches a multi-day forecast for a given location.
    /// </summary>
    /// <param name="coordinates">The coordinates for the weather forecast.</param>
    /// <param name="days">Number of days for forecast.</param>
    /// <param name="units">The units for temperature (e.g., "fahrenheit" or "celsius"). Default is "fahrenheit".</param>
    /// <returns>List of WeatherReport for each day.</returns>
    Task<IEnumerable<WeatherForecast>> GetWeatherForecast(Coordinates coordinates, int days);
}
