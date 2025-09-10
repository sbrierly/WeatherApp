using System.Text.Json.Serialization;

using YamlDotNet.Serialization;

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
    [YamlMember(Alias="currentTemperature")]
    public required int CurrentTemperature { get; set; }

    /// <summary>
    /// Unit of temperature measurement (e.g., 'F' for Fahrenheit, 'C' for Celsius).
    /// </summary>
    [JsonPropertyName("unit")]
    [YamlMember(Alias="unit")]
    public required char Unit { get; set; }

    /// <summary>
    /// Latitude of the location.
    /// </summary>
    [JsonPropertyName("lat")]
    [YamlMember(Alias="lat")]
    public required double Latitude { get; set; }

    /// <summary>
    /// Longitude of the location.
    /// </summary>
    [JsonPropertyName("lon")]
    [YamlMember(Alias="lon")]
    public required double Longitude { get; set; }

    /// <summary>
    /// Indicates if rain is possible today.
    /// </summary>
    [JsonPropertyName("rainPossibleToday")]
    [YamlMember(Alias="rainPossibleToday")]
    public required bool RainPossibleToday { get; set; }
}
