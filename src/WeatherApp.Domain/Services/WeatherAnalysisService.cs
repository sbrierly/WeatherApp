using WeatherApp.Domain.Entities;
using WeatherApp.Domain.ValueObjects;

namespace WeatherApp.Domain.Services;

/// <summary>
/// Service for analyzing weather data.
/// </summary>
public class WeatherAnalysisService : IWeatherAnalysisService
{
    /// <summary>
    /// Calculates the average temperature from a list of weather forecasts over a specified number of days and in the specified units.
    /// </summary>
    /// <param name="forecasts">Collection of weather forecasts.</param>
    /// <returns>Average temperature over the specified period.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Temperature GetAverageTemperature(IEnumerable<WeatherForecast> forecasts)
    {
        return Temperature.Average(forecasts.Select(f => f.Temperature));
    }

    /// <summary>
    /// Determines if rain is possible in the given list of weather forecasts.
    /// </summary>
    /// <param name="forecasts">Collection of weather forecasts.</param>
    /// <returns>True if rain is possible; otherwise, false.</returns>
    public bool RainPossibleInPeriod(IEnumerable<WeatherForecast> forecasts)
    {
        return forecasts.Any(f => f.Condition.ToLower().Contains("rain"));
    }
}
