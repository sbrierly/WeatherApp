using WeatherApp.Domain.ValueObjects;

namespace WeatherApp.Domain.Tests.ValueObjects
{
    public class TemperatureTests
    {
        [Fact]
        public void Constructor_StoresValueInFahrenheit()
        {
            // Arrange & Act
            var temp = new Temperature(77.0);

            // Assert
            Assert.Equal(77.0, temp.Fahrenheit);
        }

        [Theory]
        [InlineData(32.0, 0.0)]
        [InlineData(212.0, 100.0)]
        [InlineData(98.6, 37.0)]
        public void Celsius_ReturnsCorrectValue(double fahrenheit, double expectedCelsius)
        {
            // Arrange
            var temp = new Temperature(fahrenheit);

            // Act
            var celsius = temp.Celsius;

            // Assert
            Assert.Equal(expectedCelsius, celsius, 2);
        }

        [Theory]
        [InlineData(0.0, 32.0)]
        [InlineData(100.0, 212.0)]
        [InlineData(37.0, 98.6)]
        public void FromCelsius_CreatesCorrectFahrenheit(double celsius, double expectedFahrenheit)
        {
            // Arrange & Act
            var temp = Temperature.FromCelsius(celsius);

            // Assert
            Assert.Equal(expectedFahrenheit, temp.Fahrenheit, 2);
            Assert.Equal(celsius, temp.Celsius, 2);
        }

        [Theory]
        [InlineData(32.0)]
        [InlineData(212.0)]
        [InlineData(98.6)]
        public void FromFahrenheit_CreatesCorrectTemperature(double fahrenheit)
        {
            // Arrange & Act
            var temp = Temperature.FromFahrenheit(fahrenheit);

            // Assert
            Assert.Equal(fahrenheit, temp.Fahrenheit, 2);
        }

        [Fact]
        public void Average_ReturnsCorrectAverageTemperature()
        {
            // Arrange
            var temps = new[]
            {
            Temperature.FromFahrenheit(32.0),
            Temperature.FromFahrenheit(63.2),
            Temperature.FromFahrenheit(98.6)
            };

            // Act
            var avg = Temperature.Average(temps);
            var expectedAvgF = temps.Average(t => t.Fahrenheit);

            // Assert
            Assert.Equal(expectedAvgF, avg.Fahrenheit, 2);
        }

        [Fact]
        public void Average_EmptySequence_ThrowsInvalidOperationException()
        {
            // Arrange
            var temps = Enumerable.Empty<Temperature>();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => Temperature.Average(temps));
        }
    }
}
