using WeatherApp.Domain.Entities;
using WeatherApp.Domain.Services;
using WeatherApp.Domain.ValueObjects;

namespace WeatherApp.Domain.Tests.Services;
public class WeatherAnalysisServiceTest
{
    private readonly WeatherAnalysisService _service = new();

    [Fact]
    public void GetAverageTemperature_ReturnsCorrectAverage()
    {
        // Arrange
        var forecasts = new List<WeatherForecast>
        {
            new() {
                TimeStamp = DateTime.Now,
                Location = new Coordinates(1, 1),
                Temperature = Temperature.FromCelsius(10),
                Condition = "Clear"
            },
            new() {
                TimeStamp = DateTime.Now.AddHours(3),
                Location = new Coordinates(1, 1),
                Temperature = Temperature.FromCelsius(20),
                Condition = "Cloudy"
            },
            new() {
                TimeStamp = DateTime.Now.AddHours(6),
                Location = new Coordinates(1, 1),
                Temperature = Temperature.FromCelsius(30),
                Condition = "Rain"
            }
        };

        // Act
        var average = _service.GetAverageTemperature(forecasts);

        // Assert
        Assert.Equal(20, average.Celsius, 1);
    }

    [Fact]
    public void GetAverageTemperature_EmptyList_ThrowsException()
    {
        // Arrange
        var forecasts = new List<WeatherForecast>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _service.GetAverageTemperature(forecasts));
    }

    [Fact]
    public void RainPossibleInPeriod_RainPresent_ReturnsTrue()
    {
        // Arrange
        var forecasts = new List<WeatherForecast>
        {
            new() {
                TimeStamp = DateTime.Now.AddHours(3),
                Location = new Coordinates(1, 1),
                Temperature = Temperature.FromCelsius(20),
                Condition = "Cloudy"
            },
            new() {
                TimeStamp = DateTime.Now.AddHours(6),
                Location = new Coordinates(1, 1),
                Temperature = Temperature.FromCelsius(30),
                Condition = "Rain"
            }
        };

        // Act
        var result = _service.RainPossibleInPeriod(forecasts);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void RainPossibleInPeriod_NoRain_ReturnsFalse()
    {
        // Arrange
        var forecasts = new List<WeatherForecast>
        {
            new() {
                TimeStamp = DateTime.Now,
                Location = new Coordinates(1, 1),
                Temperature = Temperature.FromCelsius(10),
                Condition = "Clear"
            },
            new() {
                TimeStamp = DateTime.Now.AddHours(3),
                Location = new Coordinates(1, 1),
                Temperature = Temperature.FromCelsius(20),
                Condition = "Cloudy"
            }
        };

        // Act
        var result = _service.RainPossibleInPeriod(forecasts);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void RainPossibleInPeriod_EmptyList_ReturnsFalse()
    {
        // Arrange
        var forecasts = new List<WeatherForecast>();

        // Act
        var result = _service.RainPossibleInPeriod(forecasts);

        // Assert
        Assert.False(result);
    }
}
