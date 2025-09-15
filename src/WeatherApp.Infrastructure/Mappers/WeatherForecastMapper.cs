using WeatherApp.Domain.Entities;
using WeatherApp.Domain.ValueObjects;
using WeatherApp.Infrastructure.Models;

namespace WeatherApp.Infrastructure.Mappers;

/// <summary>
/// Mapper class for converting weather forecast data from API models to domain entities.
/// </summary>
public static class WeatherForecastMapper
{
    /// <summary>
    /// Maps OpenWeatherGetCurrentResponse to WeatherForecast domain entity.
    /// </summary>
    /// <param name="response">Response from OpenWeather API</param>
    /// <returns>A WeatherForecast domain entity</returns>
    public static WeatherForecast MapToDomain(OpenWeatherGetCurrentResponse response)
    {
        return new WeatherForecast
        {
            TimeStamp = response.DateTimeUtc,
            Temperature = Temperature.FromFahrenheit(response.Main.Temp),
            Condition = response.Weather[0].Main,
            Location = new Coordinates
            {
                Longitude = response.Coordinates.Lon,
                Latitude = response.Coordinates.Lat
            }
        };
    }

    /// <summary>
    /// Maps OpenWeatherGetForecastResponse to a collection of WeatherForecast domain entities.
    /// </summary>
    /// <param name="response">Response from OpenWeather API</param>
    /// <returns>A collection of WeatherForecast domain entities</returns>
    public static IEnumerable<WeatherForecast> MapToDomain(OpenWeatherGetForecastResponse response)
    {
        return response.Items.Select(item => new WeatherForecast
        {
            TimeStamp = item.DateTimeUtc,
            Temperature = Temperature.FromFahrenheit(item.Main.Temp),
            Condition = item.Weather.FirstOrDefault()?.Main ?? "Unknown",
            Location = new Coordinates
            {
                Longitude = response.City.Coord.Lon,
                Latitude = response.City.Coord.Lat
            }
        });
    }
}
