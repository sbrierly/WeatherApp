namespace WeatherApp.Infrastructure.Models;

/// <summary>
/// Configuration options for external APIs.
/// </summary>
public class ApiOptions
{
    /// <summary>
    /// Gets or sets the base URL of the API.
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the API key for authentication.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the cache time-to-live in minutes.
    /// </summary>
    public int CacheTTLMinutes { get; set; } = 60;
}
