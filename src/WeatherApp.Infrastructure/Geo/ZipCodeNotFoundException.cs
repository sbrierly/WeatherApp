namespace WeatherApp.Infrastructure.Geo;

[Serializable]
/// <summary>
/// Exception thrown when a provided zipcode cannot be found.
/// </summary>
public class ZipcodeNotFoundException : KeyNotFoundException
{
    public ZipcodeNotFoundException(string zipcode)
        : base($"ZIP code '{zipcode}' could not be found.") { }
}
