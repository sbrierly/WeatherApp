using WeatherApp.Domain.ValueObjects;

namespace WeatherApp.Domain.Entities
{
/// <summary>
/// Represents a weather forecast data point.
/// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// The timestamp of the forecast.
        /// </summary>
        public required DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The temperature details of the forecast.
        /// </summary>
        public required Temperature Temperature { get; set; }

        /// <summary>
        /// The condition details of the forecast.  Cloudy, Rainy, etc.
        /// </summary>
        public required string Condition { get; set; }

        /// <summary>
        /// The geographical location associated with the forecast.
        /// </summary>
        public required Coordinates Location { get; set; }
    }
}
