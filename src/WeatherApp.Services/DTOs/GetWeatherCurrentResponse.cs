using System.Text.Json.Serialization;

namespace WeatherApp.Services.DTOs;

/// <summary>
/// Response DTO for current weather data.
/// </summary>
public class GetWeatherCurrentResponse
{
    /// <summary>
    /// Current temperature.
    /// </summary>
    [JsonPropertyName("currentTemperature")]
    public int CurrentTemperature { get; set; }

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
    /// Indicates if rain is possible today.
    /// </summary>
    [JsonPropertyName("rainPossibleToday")]
    public bool RainPossibleToday { get; set; }
}
