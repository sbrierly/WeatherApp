namespace WeatherApp.Domain.ValueObjects;

public readonly record struct Temperature
{
    /// <summary>
    /// Internal value stored in Fahrenheit.
    /// </summary>
    private double _valueFahrenheit { get; }

    /// <summary>
    /// Constructor that stores the temperature in Fahrenheit.
    /// </summary>
    public Temperature(double valueFahrenheit)
    {
        _valueFahrenheit = valueFahrenheit;
    }

    /// <summary>
    /// Returns the temperature in Celsius.
    /// </summary>
    public double Celsius => (_valueFahrenheit - 32) * 5.0 / 9.0;

    /// <summary>
    /// Returns the temperature in Fahrenheit (same as Value).
    /// </summary>
    public double Fahrenheit => _valueFahrenheit;

    /// <summary>
    /// Creates a Temperature from Fahrenheit.
    /// </summary>
    public static Temperature FromFahrenheit(double fahrenheit)
    {
        return new Temperature(fahrenheit);
    }

    /// <summary>
    /// Creates a Temperature from Celsius.
    /// </summary>
    public static Temperature FromCelsius(double celsius)
    {
        double fahrenheit = (celsius * 9.0 / 5.0) + 32;
        return new Temperature(fahrenheit);
    }

    /// <summary>
    /// Averages a sequence of Temperature objects and returns a new Temperature in Fahrenheit.
    /// </summary>
    public static Temperature Average(IEnumerable<Temperature> temperatures)
    {
        double avgF = temperatures.Average(t => t._valueFahrenheit);
        return new Temperature(avgF);
    }
}



// /// <summary>
// /// Represents a temperature with a specific unit (Celsius or Fahrenheit).
// /// Provides safe conversion between units.
// /// </summary>
// public readonly record struct Temperature(double Value, TemperatureUnit Unit)
// {
//     /// <summary>
//     /// Gets the temperature in Celsius.
//     /// </summary>
//     public double Celsius =>
//         Unit == TemperatureUnit.Celsius
//             ? Value
//             : (Value - 32) * 5.0 / 9.0;

//     /// <summary>
//     /// Gets the temperature in Fahrenheit.
//     /// </summary>
//     public double Fahrenheit =>
//         Unit == TemperatureUnit.Fahrenheit
//             ? Value
//             : (Value * 9.0 / 5.0) + 32;

//     /// <summary>
//     /// Creates a Temperature in Celsius.
//     /// </summary>
//     public static Temperature FromCelsius(double celsius) =>
//         new(celsius, TemperatureUnit.Celsius);

//     /// <summary>
//     /// Creates a Temperature in Fahrenheit.
//     /// </summary>
//     public static Temperature FromFahrenheit(double fahrenheit) =>
//         new(fahrenheit, TemperatureUnit.Fahrenheit);

//     public static Temperature Average(IEnumerable<Temperature> temperatures)
//     {
//         if (temperatures == null || !temperatures.Any())
//             throw new ArgumentException("Temperature collection is null or empty", nameof(temperatures));

//         var totalFahrenheit = temperatures.Sum(t => t.Fahrenheit);
//         var averageFahrenheit = totalFahrenheit / temperatures.Count();

//         return FromFahrenheit(averageFahrenheit);
//     }
// }
