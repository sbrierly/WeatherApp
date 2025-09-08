using WeatherApp.Domain.Enums;
using WeatherApp.Services.DTOs;

namespace WeatherApp.Services.Interfaces
{
    /// <summary>
    /// Service interface for fetching weather data.
    /// </summary>
    public interface IWeatherService
    {
        /// <summary>
        /// Gets the current weather for the specified location.
        /// </summary>
        /// <param name="zipcode">The zipcode of the location.</param>
        /// <param name="units">The units for temperature (e.g., "fahrenheit" or "celsius"). Default is "fahrenheit".</param>
        /// <returns>A WeatherReport representing current weather conditions.</returns>
        Task<GetWeatherCurrentResponse> GetCurrentWeatherByZipcode(string zipcode, TemperatureUnit units = TemperatureUnit.Fahrenheit);

        /// <summary>
        /// Gets an average weather forecast for a multi-day period for the specified location.
        /// </summary>
        /// <param name="zipcode">The zipcode of the location.</param>
        /// <param name="days">The number of days for the forecast.</param>
        /// <param name="units">The units for temperature (e.g., "fahrenheit" or "celsius"). Default is "fahrenheit".</param>
        /// <returns>A WeatherReport representing the average weather conditions over the specified period.</returns>
        Task<GetWeatherAverageResponse> GetForecastByZipcode(string zipcode, int days, TemperatureUnit units = TemperatureUnit.Fahrenheit);
    }
}
