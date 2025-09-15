using System.Net.Http.Json;

using Microsoft.Extensions.Options;

using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Repositories;
using WeatherApp.Domain.ValueObjects;
using WeatherApp.Infrastructure.Mappers;
using WeatherApp.Infrastructure.Models;

namespace WeatherApp.Infrastructure.Repositories;

public class OpenWeatherRepository : IWeatherRepository
{
    private readonly HttpClient _httpClient;
    private readonly ApiOptions _options;

    /// <summary>
    /// Repository for fetching weather data from OpenWeather API.
    /// </summary>
    /// <param name="httpClient">HttpClient for making API requests.</param>
    /// <param name="options">Options for the OpenWeather API.</param>
    public OpenWeatherRepository(HttpClient httpClient, IOptionsSnapshot<ApiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Get("WeatherApi");
    }

    /// <summary>
    /// Fetches current weather for a given location (e.g., zipcode).
    /// </summary>
    /// <param name="zipcode">The location zipcode.</param>
    /// <param name="units">The units for temperature (e.g., "fahrenheit" or "celsius").</param>
    /// <returns>Weather report for the location.</returns>
    public async Task<WeatherForecast> GetCurrentWeather(Coordinates coordinates)
    {
        var url = $"{_options.BaseUrl}weather/?lat={coordinates.Latitude}&lon={coordinates.Longitude}&units=imperial&appid={_options.ApiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<OpenWeatherGetCurrentResponse>();
        var result = data ?? throw new InvalidOperationException("No weather data found for the provided coordinates");

        return await Task.FromResult(WeatherForecastMapper.MapToDomain(result));
    }

    /// <summary>
    /// Fetches a multi-day forecast for a given location.
    /// </summary>
    /// <param name="zipcode">The location zipcode.</param>
    /// <param name="days">Number of days for forecast.</param>
    /// <param name="units">The units for temperature (e.g., "fahrenheit" or "celsius").</param>
    /// <returns>List of WeatherReport for each day.</returns>
    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecast(Coordinates coordinates, int days)
    {
        int forecastCount = days * 8; // OpenWweather forecast API provides data in 3-hour intervals, so 8 intervals per day.

        var url = $"{_options.BaseUrl}forecast?lat={coordinates.Latitude}&lon={coordinates.Longitude}&cnt={forecastCount}&units=imperial&appid={_options.ApiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<OpenWeatherGetForecastResponse>();
        var result = data ?? throw new InvalidOperationException("No weather data found for the provided coordinates");

        return await Task.FromResult(WeatherForecastMapper.MapToDomain(result));
    }
}
