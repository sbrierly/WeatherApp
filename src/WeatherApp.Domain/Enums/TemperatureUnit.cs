using System.Text.Json.Serialization;

namespace WeatherApp.Domain.Enums;

/// <summary>
/// Units for temperature measurement.
/// </summary>
public enum TemperatureUnit
{
    [JsonPropertyName("fahrenheit")]
    Fahrenheit,
    [JsonPropertyName("celsius")]
    Celsius
}
