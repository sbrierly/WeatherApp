using System.Text.Json.Serialization;

namespace WeatherApp.Infrastructure.Models;

public class OpenWeatherGetCurrentResponse
{
    [JsonPropertyName("dt")]
    public long Timestamp { get; set; }

    [JsonIgnore]
    public DateTime DateTimeUtc => DateTimeOffset.FromUnixTimeSeconds(Timestamp).UtcDateTime;

    [JsonPropertyName("main")]
    public required MainInfo Main { get; set; }

    [JsonPropertyName("weather")]
    public required WeatherInfo[] Weather { get; set; }

    [JsonPropertyName("coord")]
    public required Coord Coordinates { get; set; }

    public class MainInfo
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; }
    }

    public class WeatherInfo
    {
        [JsonPropertyName("main")]
        public required string Main { get; set; }
    }

    public class Coord
    {
        [JsonPropertyName("lon")]
        public double Lon { get; set; }

        [JsonPropertyName("lat")]
        public double Lat { get; set; }
    }
}
