using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

using WeatherApp.Domain.Enums;
using WeatherApp.Infrastructure.Geo;
using WeatherApp.Services.DTOs;
using WeatherApp.Services.Interfaces;

namespace WeatherApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
/// <summary>
/// Controller to handle weather-related HTTP requests.
/// </summary>
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;
    private readonly ILogger<WeatherController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="WeatherController"/> class.
    /// </summary>
    /// <param name="weatherService"></param>
    /// <param name="logger"></param>
    public WeatherController(IWeatherService weatherService, ILogger<WeatherController> logger)
    {
        _weatherService = weatherService;
        _logger = logger;
    }

    [HttpGet("Current/{zipcode}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    /// <summary>
    /// Gets the current weather for the specified zipcode.
    /// </summary>
    /// <param name="zipcode">The location zipcode</param>
    /// <param name="units">The units for temperature (e.g., "fahrenheit" or "celsius"). Default is "fahrenheit".</param>
    /// <returns>The current weather report</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetWeatherCurrentResponse))]
    public async Task<IActionResult> Get(
        [FromRoute, RegularExpression("^[0-9]{5}$")] string zipcode,
        [FromQuery] TemperatureUnit units = TemperatureUnit.Fahrenheit)
    {
        try
        {
            _logger.LogInformation("Fetching current weather for zipcode {Zipcode} with units {Units}", zipcode, units);
            var result = await _weatherService.GetCurrentWeatherByZipcode(zipcode, units);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return _handleException(ex, zipcode);
        }
    }

    [HttpGet("Average/{zipcode}")]
    /// <summary>
    /// Gets an average weather forecast for a multi-day period for the specified zipcode.
    /// </summary>
    /// <param name="zipcode">The location zipcode</param>
    /// <param name="days">Number of days for forecast</param>
    /// <param name="units">The units for temperature (e.g., "fahrenheit" or "celsius"). Default is "fahrenheit".</param>
    /// <returns>The average weather report</returns>
    public async Task<IActionResult> Get(
        [FromRoute, RegularExpression("^[0-9]{5}$")] string zipcode,
        [FromQuery, Range(2, 5)] int days,
        [FromQuery] TemperatureUnit units = TemperatureUnit.Fahrenheit)
    {
        try
        {
            _logger.LogInformation("Fetching {Days}-day average weather forecast for zipcode {Zipcode} with units {Units}", days, zipcode, units);
            var result = await _weatherService.GetForecastByZipcode(zipcode, days, units);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return _handleException(ex, zipcode);
        }
    }

    private IActionResult _handleException(Exception ex, string zipcode)
    {
        _logger.LogError(ex, "Error occurred while logging request for zipcode {Zipcode}", zipcode);

        if (ex is ZipcodeNotFoundException)
            // TODO: consider returning 404 NotFound instead
            return BadRequest(ProblemDetailsFactory.CreateProblemDetails(HttpContext, StatusCodes.Status400BadRequest, title: ex.Message));
        else
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                ProblemDetailsFactory.CreateProblemDetails(
                    HttpContext, StatusCodes.Status500InternalServerError, title: "An error occurred while processing your request."));
    }
}
