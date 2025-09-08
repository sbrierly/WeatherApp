using WeatherApp.Domain.Entities;
using WeatherApp.Domain.ValueObjects;

namespace WeatherApp.Domain.Services;

public interface IWeatherAnalysisService
{
    /// <summary>
    /// Calculates the average temperature from a list of weather forecasts over a specified number of days and in the specified units.
    /// </summary>
    /// <param name="forecasts">Collection of weather forecasts.</param>
    /// <param name="days">Number of days to consider for the average.</param>
    /// <param name="units">Temperature units to use for the calculation.</param>
    /// <returns>Average temperature over the specified period.</returns>
    Temperature GetAverageTemperature(IEnumerable<WeatherForecast> forecasts);

    /// <summary>
    /// Determines if rain is possible in the given list of weather forecasts.
    /// </summary>
    /// <param name="forecasts">Collection of weather forecasts.</param>
    /// <returns>True if rain is possible; otherwise, false.</returns>
    bool RainPossibleInPeriod(IEnumerable<WeatherForecast> forecasts);
}
