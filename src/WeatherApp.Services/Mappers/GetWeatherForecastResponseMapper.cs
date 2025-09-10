namespace WeatherApp.Services.Mappers;

using WeatherApp.Domain.Enums;
using WeatherApp.Domain.ValueObjects;
using WeatherApp.Services.DTOs;

class GetWeatherForecastResponseMapper
{

    public static GetWeatherAverageResponse MapToDto(Coordinates coordinates, TemperatureUnit units, Temperature averageTemp, bool rainPossible)
    {
        return new GetWeatherAverageResponse
        {
            AverageTemperature = (int)Math.Round(units.ToString().Equals("fahrenheit", StringComparison.CurrentCultureIgnoreCase) ? averageTemp.Fahrenheit : averageTemp.Celsius),
            Unit = char.ToUpper(units.ToString()[0]),
            RainPossibleInPeriod = rainPossible,
            Longitude = coordinates.Longitude,
            Latitude = coordinates.Latitude
        };
    }
}
