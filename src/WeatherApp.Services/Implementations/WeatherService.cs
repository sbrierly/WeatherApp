using WeatherApp.Domain.Enums;
using WeatherApp.Domain.Repositories;
using WeatherApp.Domain.Services;
using WeatherApp.Services.DTOs;
using WeatherApp.Services.Interfaces;
using WeatherApp.Services.Mappers;

namespace WeatherApp.Services.Implementations;

/// <summary>
/// Service layer that orchestrates domain entities and repositories for weather operations.
/// </summary>
public class WeatherService : IWeatherService
{
    private readonly IWeatherRepository _repository;
    private readonly IGeocodingService _geocodingService;
    private readonly IWeatherAnalysisService _weatherAnalysisService;

    /// <summary>
    /// Constructor for WeatherService.
    /// </summary>
    /// <param name="repository">WeatherRepository for accessing weather data.</param>
    /// <param name="geocodingService">GeocodingService for converting addresses to coordinates.</param>
    /// <param name="weatherAnalysisService">WeatherAnalysisService for analyzing weather data.</param>
    public WeatherService(IWeatherRepository repository, IGeocodingService geocodingService, IWeatherAnalysisService weatherAnalysisService)
    {
        _repository = repository;
        _geocodingService = geocodingService;
        _weatherAnalysisService = weatherAnalysisService;
    }

    /// <summary>
    /// Gets the current weather for the specified zipcode.
    /// <param name="zipcode">The zipcode of the location.</param>
    /// <param name="units">The units for temperature (e.g., "fahrenheit" or "celsius").</param>
    /// <returns>A WeatherReport representing current weather conditions.</returns>
    /// </summary>
    public async Task<GetWeatherCurrentResponse> GetCurrentWeatherByZipcode(string zipcode, TemperatureUnit units)
    {
        var coordinates = await _geocodingService.GetCoordinatesAsync(zipcode);
        var current = await _repository.GetCurrentWeather(coordinates) ?? throw new InvalidOperationException($"No weather data found for zipcode {zipcode}");

        return GetWeatherCurrentResponseMapper.MapToDto(current, units);
    }

    /// <summary>
    /// Gets an average weather forecast for a multi-day period for the specified location.
    /// </summary>
    /// <param name="zipcode">The zipcode of the location.</param>
    /// <param name="days">The number of days to forecast.</param>
    /// <param name="units">The units for temperature (e.g., "fahrenheit" or "celsius").</param>
    /// <returns>A GetWeatherAverageResponse containing the average weather data.</returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<GetWeatherAverageResponse> GetForecastByZipcode(string zipcode, int days, TemperatureUnit units)
    {
        var coordinates = await _geocodingService.GetCoordinatesAsync(zipcode);
        var forecast = await _repository.GetWeatherForecast(coordinates, days);

        var averageTemp = _weatherAnalysisService.GetAverageTemperature(forecast);
        var rainPossible = _weatherAnalysisService.RainPossibleInPeriod(forecast);

        return new GetWeatherAverageResponse
        {
            AverageTemperature = (int)Math.Round(units.ToString().ToLower() == "fahrenheit" ? averageTemp.Fahrenheit : averageTemp.Celsius),
            Unit = char.ToUpper(units.ToString()[0]),
            RainPossibleInPeriod = rainPossible,
            Longitude = coordinates.Longitude,
            Latitude = coordinates.Latitude
        };
    }
}
