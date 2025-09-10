namespace WeatherApp.Domain.ValueObjects;

/// <summary>
/// Represents geographical coordinates with latitude and longitude.
/// </summary>
/// <param name="Latitude">The latitude of the location.</param>
/// <param name="Longitude">The longitude of the location.</param>
public readonly record struct Coordinates(double Latitude, double Longitude);
