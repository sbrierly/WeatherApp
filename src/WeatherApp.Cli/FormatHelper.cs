using System.Text.Json;

using WeatherApp.Cli.Enums;
using WeatherApp.Services.DTOs;
using YamlDotNet.Serialization;

namespace WeatherApp.Cli;

/// <summary>
/// Output format helper for support types - TEXT | JSON | YAML.
/// </summary>
public static class FormatHelper
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions;
    private static readonly ISerializer _yamlSerializer;

    static FormatHelper()
    {
        _jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };
        _yamlSerializer = new SerializerBuilder().Build();
    }

    /// <summary>
    /// Generates a formatted output string for a given zipcode and content, 
    /// using the specified output format.
    /// </summary>
    /// <typeparam name="T">The type of object that <paramref name="content"/> represents when deserialized.</typeparam>
    /// <param name="zipcode">The zipcode for which the output is being generated.</param>
    /// <param name="content">The raw content to format. Typically a serialized representation (e.g., JSON) 
    /// of an object of type <typeparamref name="T"/>.</param>
    /// <param name="output">The desired output format (e.g., text, JSON, YAML).</param>
    /// <returns>
    /// A string representing the formatted output according to the specified <paramref name="output"/> format.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="content"/> is null or empty when a non-null value is required.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// Thrown if <typeparamref name="T"/> or <paramref name="output"/> is not supported by this method.
    /// </exception>
    public static string GetFormattedOutput<T>(string zipcode, string content, OutputFormat output)
    {
        var serializedContent = JsonSerializer.Deserialize<T>(content);

        return output switch
        {
            OutputFormat.TEXT => GetTextOutput(zipcode, serializedContent),
            OutputFormat.JSON => GetJsonOutput(serializedContent),
            OutputFormat.YAML => GetYamlOutput(serializedContent),
            _ => throw new InvalidOperationException(""),
        };
    }

    private static string GetTextOutput<T>(string zipcode, T content)
    {
        return content switch
        {
            GetWeatherCurrentResponse currentWeatherContent => GetCurrentWeatherTextOutput(zipcode, currentWeatherContent),
            GetWeatherAverageResponse averageWeatherContent => GetAverageWeatherTextOutput(zipcode, averageWeatherContent),
            _ => throw new NotSupportedException($"Type {typeof(T).Name} not supported.")
        };
    }

    private static string GetCurrentWeatherTextOutput(string zipcode, GetWeatherCurrentResponse content)
    {
        return $"Location: {zipcode}\n" +  // Environment.NewLine unnecessary, CLI honors '\n' cross platform.
               $"Current Temperature: {content?.CurrentTemperature}°{content?.Unit}\n" +
               $"Rain Possible Today: {content?.RainPossibleToday}\n";
    }

    private static string GetAverageWeatherTextOutput(string zipcode, GetWeatherAverageResponse content)
    {
        return $"Location: {zipcode}\n" +
               $"Average Temperature: {content?.AverageTemperature}°{content?.Unit}\n" +
               $"Rain Possible In Period: {content?.RainPossibleInPeriod}";
    }

    private static string GetJsonOutput<T>(T content)
    {
        return JsonSerializer.Serialize<T>(content, _jsonSerializerOptions);
    }

    private static string GetYamlOutput<T>(T content)
    {
        return _yamlSerializer.Serialize(content);
    }
}
