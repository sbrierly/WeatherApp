using System.Text.Json.Serialization;

namespace WeatherApp.Services.DTOs;
/// <summary>
/// Response DTO for average weather data.
/// </summary>
public class GetWeatherAverageResponse
{
    /// <summary>
    /// Average temperature over the specified period.
    /// </summary>
    [JsonPropertyName("averageTemperature")]
    public int AverageTemperature { get; set; }

    /// <summary>
    /// Unit of temperature measurement (e.g., 'F' for Fahrenheit, 'C' for Celsius).
    /// </summary>
    [JsonPropertyName("unit")]
    public required char Unit { get; set; }

    /// <summary>
    /// Latitude of the location.
    /// </summary>
    [JsonPropertyName("lat")]
    public required double Latitude { get; set; }

    /// <summary>
    /// Longitude of the location.
    /// </summary>
    [JsonPropertyName("lon")]
    public double Longitude { get; set; }

    /// <summary>
    /// Indicates if rain is possible during the forecasted period.
    /// </summary>
    [JsonPropertyName("rainPossibleInPeriod")]
    public bool RainPossibleInPeriod { get; set; }
}
