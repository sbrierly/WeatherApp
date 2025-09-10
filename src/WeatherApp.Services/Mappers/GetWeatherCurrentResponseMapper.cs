namespace WeatherApp.Services.Mappers;

using Domain.Entities;
using WeatherApp.Domain.Enums;
using WeatherApp.Services.DTOs;

class GetWeatherCurrentResponseMapper
{
    public static GetWeatherCurrentResponse MapToDto(WeatherForecast forecast, TemperatureUnit units)
    {
        return new GetWeatherCurrentResponse
        {
            CurrentTemperature = (int)Math.Round(units == TemperatureUnit.Fahrenheit ? forecast.Temperature.Fahrenheit : forecast.Temperature.Celsius),
            RainPossibleToday = forecast.Condition.Contains("rain", StringComparison.CurrentCultureIgnoreCase),
            Unit = char.ToUpper(units.ToString()[0]),
            Latitude = forecast.Location.Latitude,
            Longitude = forecast.Location.Longitude
        };
    }
}
