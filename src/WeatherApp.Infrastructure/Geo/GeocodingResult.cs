using System.Text.Json.Serialization;

namespace WeatherApp.Infrastructure.Geo;

/// <summary>
/// Represents the result from a geocoding API.
/// </summary>
class GeocodingResult
{
    /// <summary>
    /// Gets or sets the latitude of the location.
    /// </summary>
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }

    /// <summary>
    /// Gets or sets the longitude of the location.
    /// </summary>
    [JsonPropertyName("lon")]
    public double Longitude { get; set; }
}
