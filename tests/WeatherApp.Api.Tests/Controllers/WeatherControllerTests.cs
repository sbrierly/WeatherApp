using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Moq;

using WeatherApp.Api.Controllers;
using WeatherApp.Services.DTOs;
using WeatherApp.Services.Interfaces;
using WeatherApp.Domain.Enums;
using WeatherApp.Infrastructure.Geo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace WeatherApp.API.Tests.Controllers
{
    public class WeatherControllerTests
    {
        private readonly Mock<IWeatherService> _mockWeatherService;
        private readonly Mock<ILogger<WeatherController>> _mockLogger;
        private readonly WeatherController _controller;

        public WeatherControllerTests()
        {
            _mockWeatherService = new Mock<IWeatherService>();
            _mockLogger = new Mock<ILogger<WeatherController>>();

            var mockFactory = new Mock<ProblemDetailsFactory>();
            mockFactory
                .Setup(f => f.CreateProblemDetails(
                    It.IsAny<HttpContext>(),
                    It.IsAny<int?>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(new ProblemDetails());

            _controller = new WeatherController(_mockWeatherService.Object, _mockLogger.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext
                    {
                        RequestServices = new ServiceCollection()
                            .AddSingleton(mockFactory.Object)
                            .BuildServiceProvider()
                    }
                }
            };
        }

        [Fact]
        public async Task Get_CurrentWeather_ReturnsOk()
        {
            // Arrange
            var zipcode = "12345";
            var response = new GetWeatherCurrentResponse
            {
                CurrentTemperature = 70,
                Unit = 'F',
                Latitude = 0.0
            };
            _mockWeatherService
                .Setup(s => s.GetCurrentWeatherByZipcode(zipcode, TemperatureUnit.Fahrenheit))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Get(zipcode);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response, okResult.Value);
        }

        [Fact]
        public async Task Get_CurrentWeather_ZipcodeNotFound_ReturnsBadRequest()
        {
            // Arrange
            var zipcode = "99999";
            _mockWeatherService
                .Setup(s => s.GetCurrentWeatherByZipcode(zipcode, TemperatureUnit.Fahrenheit))
                .ThrowsAsync(new ZipcodeNotFoundException(zipcode));

            // Act
            var result = await _controller.Get(zipcode);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Get_CurrentWeather_ThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var zipcode = "12345";
            _mockWeatherService
                .Setup(s => s.GetCurrentWeatherByZipcode(zipcode, TemperatureUnit.Fahrenheit))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.Get(zipcode);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusResult.StatusCode);
        }

        [Fact]
        public async Task Get_AverageForecast_ReturnsOk()
        {
            // Arrange
            var zipcode = "12345";
            int days = 3;
            var response = new GetWeatherAverageResponse
            {
                AverageTemperature = 72,
                Unit = 'F',
                Latitude = 0.0
            };
            _mockWeatherService
                .Setup(s => s.GetForecastByZipcode(zipcode, days, TemperatureUnit.Fahrenheit))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Get(zipcode, days);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response, okResult.Value);
        }

        [Fact]
        public async Task Get_AverageForecast_ThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var zipcode = "12345";
            int days = 3;
            _mockWeatherService
                .Setup(s => s.GetForecastByZipcode(zipcode, days, TemperatureUnit.Fahrenheit))
                .ThrowsAsync(new Exception("Unexpected"));

            // Act
            var result = await _controller.Get(zipcode, days);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusResult.StatusCode);
        }
    }
}
